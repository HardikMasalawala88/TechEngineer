﻿using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.ObjectMapping;
using Abp.Runtime.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechEngineer.Authorization;
using TechEngineer.Authorization.Roles;
using TechEngineer.Authorization.Users;
using TechEngineer.DBEntities.Appointments.Dto;
using TechEngineer.DBEntities.Assets;
using TechEngineer.DBEntities.Location;
using TechEngineer.DBEntities.Organization;

namespace TechEngineer.DBEntities.Appointments
{
    /// <summary>
    /// Class to define appointment app service.
    /// </summary>
    [AbpAuthorize(PermissionNames.Pages_Appointments)]
    public class AppointmentAppService : AsyncCrudAppService<AppointmentEntity, AppointmentDto, Guid, PagedAppointmentResultRequestDto, CreateAppointmentDto, AppointmentDto>, IAppointmentAppService
    {
        private readonly IObjectMapper _objectMapper;
        private readonly IRepository<AppointmentEntity, Guid> _appointmentRepository;
        private readonly IRepository<AssetEntity, Guid> _assetsRepository;
        private readonly IRepository<LocationEntity, Guid> _locationsRepository;
        private readonly IRepository<OrganizationEntity, Guid> _orgRepository;
        private readonly UserManager _userManager;
        private readonly IAbpSession _abpSession;

        /// <summary>
        /// Constructor for appointment service.
        /// </summary>
        /// <param name="repository">Repository for appointment entity.</param>
        /// <param name="abpSession">abpSession.</param>
        /// <param name="userManager">User Manager.</param>
        /// <param name="assetsRepository">Assets repository.</param>
        /// <param name="locationsRepository">Locations repository.</param>
        /// <param name="orgRepository">Organizations repository.</param>
        /// <param name="objectMapper">Object mapper.</param>
        public AppointmentAppService(IRepository<AppointmentEntity, Guid> repository,
            IRepository<AssetEntity, Guid> assetsRepository,
            IRepository<LocationEntity, Guid> locationsRepository,
            IRepository<OrganizationEntity, Guid> orgRepository,
            IAbpSession abpSession,
            UserManager userManager,
            IObjectMapper objectMapper) : base(repository)
        {
            _objectMapper = objectMapper;
            _assetsRepository = assetsRepository;
            _locationsRepository = locationsRepository;
            _appointmentRepository = repository;
            _orgRepository = orgRepository;
            _userManager = userManager;
            _abpSession = abpSession;
        }

        /// <summary>
        /// Method to create appointment.
        /// </summary>
        /// <param name="input">appointment input data.</param>
        /// <returns>Return appointment.</returns>
        [AbpAuthorize(PermissionNames.Pages_Appointments_Add)]
        public override async Task<AppointmentDto> CreateAsync(CreateAppointmentDto input)
        {
            CheckCreatePermission();

            input.RequestDate = DateTime.UtcNow;
            var appointment = _objectMapper.Map<AppointmentEntity>(input);
            await _appointmentRepository.InsertAsync(appointment);
            CurrentUnitOfWork.SaveChanges();

            return MapToEntityDto(appointment);
        }

        /// <summary>
        /// Method to create filtered query.
        /// </summary>
        /// <param name="input">input parameter.</param>
        /// <returns>Return appointment entity.</returns>
        [AbpAuthorize(PermissionNames.Pages_Search_Records)]
        protected override IQueryable<AppointmentEntity> CreateFilteredQuery(PagedAppointmentResultRequestDto input)
        {
            // Appointment data searching through "Status" and "Remarks" field only.

            return Repository.GetAllIncluding(x => x.Organization, x => x.Asset, x => x.Location, x => x.User)
            .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), x => x.Status.Contains(input.Keyword) || x.Remarks.Contains(input.Keyword));
        }

        /// <summary>
        /// Method to update appointment.
        /// </summary>
        /// <param name="appointment">Appointment data.</param>
        /// <returns>Return appointment dto.</returns>
        [AbpAuthorize(PermissionNames.Pages_Appointments_Edit)]
        public override async Task<AppointmentDto> UpdateAsync(AppointmentDto appointment)
        {
            CheckUpdatePermission();

            var appointmentData = await _appointmentRepository.GetAsync(appointment.Id);
            _objectMapper.Map(appointment, appointmentData);
            await _appointmentRepository.UpdateAsync(appointmentData);

            return MapToEntityDto(appointmentData);
        }

        /// <summary>
        /// Method to get all appointments.
        /// </summary>
        /// <param name="input">Paged appointment result request dto.</param>
        /// <returns>Return appointments.</returns>
        [AbpAuthorize(PermissionNames.Pages_Appointments_List)]
        public override async Task<PagedResultDto<AppointmentDto>> GetAllAsync(PagedAppointmentResultRequestDto input)
        {
            var currentUser = await _userManager.GetUserByIdAsync(_abpSession.GetUserId());
            var roles = await _userManager.GetRolesAsync(currentUser);
            if (roles.Contains(StaticRoleNames.Tenants.SuperAdmin))
            {
                if (input.OrganizationId.HasValue && input.OrganizationId != Guid.Empty)
                {
                    var t = (input.AssetId.HasValue && input.AssetId != Guid.Empty) ?
                    Repository.GetAllIncluding(x => x.Asset).Where(x => x.AssetId == input.AssetId).PageBy(input).ToList() :
                    Repository.GetAllIncluding(x => x.Location).Where(x => x.UserId == input.UserId && x.OrganizationId == input.OrganizationId && x.LocationId == input.LocationId).PageBy(input).ToList();

                    return new PagedResultDto<AppointmentDto>
                    {
                        TotalCount = t.Count(),
                        Items = _objectMapper.Map<List<AppointmentDto>>(t)
                    };
                }
                return await base.GetAllAsync(input);
            }
            else if (roles.Contains(StaticRoleNames.Tenants.OrganizationITHead))
            {
                var t = (input.AssetId.HasValue && input.AssetId != Guid.Empty) ?
                    Repository.GetAllIncluding(x => x.Organization, x => x.Asset, x => x.Location)
                        .Where(x => x.AssetId == input.AssetId).PageBy(input).ToList() :
                    Repository.GetAllIncluding(x => x.Organization, x => x.Asset, x => x.Location, x => x.User)
                        .Where(x => x.OrganizationId == currentUser.OrganizationId || x.LocationId == currentUser.LocationId).PageBy(input).ToList();

                return new PagedResultDto<AppointmentDto>
                {
                    TotalCount = t.Count(),
                    Items = _objectMapper.Map<List<AppointmentDto>>(t)
                };
            }
            else
            {
                var t = (input.AssetId.HasValue && input.AssetId != Guid.Empty) ?
                    Repository.GetAllIncluding(x => x.Organization, x => x.Asset, x => x.Location)
                        .Where(x => x.AssetId == input.AssetId).PageBy(input).ToList() :
                    Repository.GetAllIncluding(x => x.Organization, x => x.Asset, x => x.Location, x => x.User)
                        .Where(x => x.UserId == currentUser.Id && x.OrganizationId == currentUser.OrganizationId && x.LocationId == currentUser.LocationId).PageBy(input).ToList();

                return new PagedResultDto<AppointmentDto>
                {
                    TotalCount = t.Count(),
                    Items = _objectMapper.Map<List<AppointmentDto>>(t)
                };
            }
        }

        /// <summary>
        /// Method to get list of appointment by date.
        /// </summary>
        /// <param name="RequestedDate">Request Date.</param>
        /// <returns>Return list of appointment.</returns>
        public async Task<List<AppointmentEntity>> GetListOfAppointmentByDate(DateTimeOffset RequestedDate)
        {
            List<AppointmentEntity> appointmentDtos = new List<AppointmentEntity>();

            var currentUser = await _userManager.GetUserByIdAsync(_abpSession.GetUserId());
            var roles = await _userManager.GetRolesAsync(currentUser);
            if (roles.Contains(StaticRoleNames.Tenants.SuperAdmin))
            {
                appointmentDtos = Repository.GetAll().Where(x => x.RequestDate.Date == RequestedDate.UtcDateTime).OrderBy(x => x.RequestDate).ToList();
            }

            return appointmentDtos;
        }

        /// <summary>
        /// Method to get list of assets.
        /// </summary>
        /// <returns>Return list of assets.</returns>
        public async Task<ListResultDto<AppointmentDto>> GetAppointmentsAsync()
        {
            var appointments = await Repository.GetAllListAsync();
            return new ListResultDto<AppointmentDto>(ObjectMapper.Map<List<AppointmentDto>>(appointments));
        }

        /// <summary>
        /// Method to get appointment for edit.
        /// </summary>
        /// <param name="input">Input parameter.</param>
        /// <returns>Return appointment data.</returns>
        public async Task<AppointmentDto> GetAppointmentForEdit(EntityDto<Guid> input)
        {
            var appointment = await GetAsync(input);
            return appointment;
        }
    }
}
