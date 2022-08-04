using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechEngineer.DBEntities.Appointments.Dto;

namespace TechEngineer.DBEntities.Appointments
{
    /// <summary>
    /// Interface to define appoinement app service.
    /// </summary>
    public interface IAppointmentAppService : IAsyncCrudAppService<AppointmentDto, Guid, PagedAppointmentResultRequestDto, CreateAppointmentDto, AppointmentDto>
    {
        /// <summary>
        /// Method to get list of appointment by date.
        /// </summary>
        /// <param name="RequestedDate">Request Date.</param>
        /// <returns>Return list of appointment.</returns>
        Task<List<AppointmentEntity>> GetListOfAppointmentByDate(DateTimeOffset RequestedDate);

        /// <summary>
        /// Method to get appointments.
        /// </summary>
        /// <returns>Return list of appointment.</returns>
        Task<ListResultDto<AppointmentDto>> GetAppointmentsAsync();

        /// <summary>
        /// Method to get appointment for edit.
        /// </summary>
        /// <param name="input">Input parameter.</param>
        /// <returns>Return appointment data.</returns>
        Task<AppointmentDto> GetAppointmentForEdit(EntityDto<Guid> input);
    }
}
