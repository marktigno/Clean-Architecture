using Domain.Repositories;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Data;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DockerMsSqlConnectionString")));
            // Use this if running on MSSQLLocalDB
            // services.AddDbContext<ApplicationDbContext>(options =>
            //     options.UseSqlServer(configuration.GetConnectionString("LocalDbConnectionString")));
            services.AddScoped<IUnitOfWork>(factory => factory.GetRequiredService<ApplicationDbContext>());
            services.AddScoped<IDbConnection>(factory => factory.GetRequiredService<ApplicationDbContext>().Database.GetDbConnection());
            services.AddScoped<IRepository, TodoEntryRepository>();

            return services;
        }
    }
}
