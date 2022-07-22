using System.Collections.Generic;
using TechEngineer.Roles.Dto;

namespace TechEngineer.Web.Models.Common
{
    public interface IPermissionsEditViewModel
    {
        List<FlatPermissionDto> Permissions { get; set; }
    }
}