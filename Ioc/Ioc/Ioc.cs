
using Context;
using Context.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Repository;
using Repository.Interface;
using Service;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ioc
{
    public static class Ioc
    {
        public static IServiceCollection ConfigureInjectionDependencyRepository(this IServiceCollection services)
        {
            services.AddScoped<UserIRepository, UserRepository>();
            services.AddScoped<AdressIRepository, AdressRepository>();
            services.AddScoped<RoleIRepository, RoleRepository>();
            services.AddScoped<RoleUserIRepository, RoleUserRepository>();

            return services;
        }
        public static IServiceCollection ConfigureInjectionDependencyService(this IServiceCollection services)
        {
            services.AddScoped<UserIService, UserService>();
            services.AddScoped<RoleIService, RoleService>();
            services.AddScoped<AdressIService, AdressService>();
            services.AddScoped<ConnectionIService, ConnectionService>();


            return services;
        }
        public static IServiceCollection ConfigureDBContext(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("BddConnection");
            services.AddDbContext<UserMicroServiceIDbContext, UserMicroServiceDbContext>(options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
            .LogTo(Console.WriteLine, LogLevel.Information)
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors());

            return services;
        }

    }
}
