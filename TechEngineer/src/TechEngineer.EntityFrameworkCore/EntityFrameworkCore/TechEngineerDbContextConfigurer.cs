using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace TechEngineer.EntityFrameworkCore
{
    public static class TechEngineerDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<TechEngineerDbContext> builder, string connectionString)
        {
            var serverVersion = ServerVersion.AutoDetect(connectionString);
            builder.UseMySql(connectionString, serverVersion);
        }

        public static void Configure(DbContextOptionsBuilder<TechEngineerDbContext> builder, DbConnection connection)
        {
            var serverVersion = ServerVersion.AutoDetect(connection.ConnectionString);
            builder.UseMySql(connection, serverVersion);
        }
    }
}