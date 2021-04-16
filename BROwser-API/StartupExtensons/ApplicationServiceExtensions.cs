using Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BROwser_API.StartupExtensons
{
    /// <summary>
    /// Static class which contains the static method to handle the application service configurations
    /// like database connections and service scope definitions
    /// </summary>
    public static class ApplicationServiceExtensions
    {
        /// <summary>
        /// Handling the database connections and other services and service scopes and configurations
        /// </summary>
        /// <param name="services">IServiceCollection</param>
        /// <param name="configuration">IConfiguration</param>
        /// <returns>Services</returns>
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Database connection
            services.AddDbContext<DataContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "BROwser_API", Version = "v1" });
            });

            return services;
        }
    }
}
