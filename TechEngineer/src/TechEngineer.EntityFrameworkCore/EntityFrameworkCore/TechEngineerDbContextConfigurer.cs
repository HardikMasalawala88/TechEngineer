using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace TechEngineer.EntityFrameworkCore
{
    public static class TechEngineerDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<TechEngineerDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<TechEngineerDbContext> builder, DbConnection connection)
        {
            builder.UseSqlServer(connection);
        }
    }
}
