using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace Icon.EntityFrameworkCore
{
    public static class IconDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<IconDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<IconDbContext> builder, DbConnection connection)
        {
            builder.UseSqlServer(connection);
        }
    }
}