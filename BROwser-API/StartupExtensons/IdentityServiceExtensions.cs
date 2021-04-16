using Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BROwser_API.StartupExtensons
{
    /// <summary>
    /// Static class of the extended Identity settings, which contains the user identity, 
    /// authentication and custom policies
    /// </summary>
    public static class IdentityServiceExtensions
    {
        /// <summary>
        /// Handling the User Identity settings and Authentuication like JWT settings,
        /// plus the custom policy settings
        /// </summary>
        /// <param name="services">IServiceCollection</param>
        /// <param name="configuration">IConfiguration</param>
        /// <returns>Services</returns>
        public static IServiceCollection AddIndentityServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Identity user configurations
            services.AddIdentityCore<AppUser>(options =>
            {
                options.Password.RequireNonAlphanumeric = false;
            })
            .AddEntityFrameworkStores<DataContext>()
            .AddSignInManager<SignInManager<AppUser>>();

            services.AddAuthentication();

            return services;
        }
    }
}
