using Abp.Application.Services.Dto;
using System;

namespace TechEngineer.Users.Dto
{
    /// <summary>
    /// custom PagedResultRequestDto
    /// </summary>
    public class PagedUserResultRequestDto : PagedResultRequestDto
    {
        public string Keyword { get; set; }
        public bool? IsActive { get; set; }

        /// <summary>
        /// Gets or sets a value inidicating organization.
        /// </summary>
        public Guid? OrganizationId { get; set; }
    }
}
