using Domain.Repositories;
using Infrastructure.Database;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Data;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Use this if running on Docker MSSQL
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DockerMsSqlConnectionString")));

            // Use this if running on MSSQLLocalDB
            //services.AddDbContext<ApplicationDbContext>(options =>
            //    options.UseSqlServer(configuration.GetConnectionString("LocalDbConnectionString")));

            // Use this if running on Docker Postgres
            //services.AddDbContext<ApplicationDbContext>(
            //options => options
            //    .UseNpgsql(configuration.GetConnectionString("DockerPostgresConnectionString"), npgsqlOptions =>
            //        npgsqlOptions.MigrationsHistoryTable(HistoryRepository.DefaultTableName, Schemas.Default))
            //    .UseSnakeCaseNamingConvention());

            services.AddScoped<IUnitOfWork>(factory => factory.GetRequiredService<ApplicationDbContext>());
            services.AddScoped<IDbConnection>(factory => factory.GetRequiredService<ApplicationDbContext>().Database.GetDbConnection());
            services.AddScoped<IRepository, TodoEntryRepository>();

            return services;
        }
    }
}
