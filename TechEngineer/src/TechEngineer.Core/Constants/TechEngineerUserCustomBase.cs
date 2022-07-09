using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechEngineer.Constants
{
    public abstract class TechEngineerUserCustomBase
    {
        /// <summary>
        ///   UserName of the super admin. super admin can not be deleted and UserName of the super admin can not be changed.
        /// </summary>
        public const string SuperAdminUserName = "superadmin";
        /// <summary>
        ///   Email address of the super admin. Email address of the super admin can not be changed.
        /// </summary>
        public const string SuperAdminEmailAddress = "superadmin@techengine.com";
    }

    public abstract class TechEngineerGlobalMethod
    {
        public static EntityDto<Guid> ToEntityDto(Guid guid)
        {
            EntityDto<Guid> entityDtoGuid = new EntityDto<Guid>();
            entityDtoGuid.Id = guid;
            return entityDtoGuid;
        }

        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
