using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using TechEngineer.Authorization.Roles;
using TechEngineer.Authorization.Users;
using TechEngineer.MultiTenancy;
using TechEngineer.DBEntities.Organization;
using TechEngineer.DBEntities.Location;

namespace TechEngineer.EntityFrameworkCore
{
    public class TechEngineerDbContext : AbpZeroDbContext<Tenant, Role, User, TechEngineerDbContext>
    {
        /* Define a DbSet for each entity of the application */
        public DbSet<OrganizationEntity> Organizations { get; set; }
        public DbSet<LocationEntity> Locations { get; set; }


        public TechEngineerDbContext(DbContextOptions<TechEngineerDbContext> options)
            : base(options)
        {
        }
    }
}
