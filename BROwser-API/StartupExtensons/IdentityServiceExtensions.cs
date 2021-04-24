using Database;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
             .AddRoles<AppRole>()
             .AddRoleManager<RoleManager<AppRole>>()
             .AddSignInManager<SignInManager<AppUser>>()
             .AddRoleValidator<RoleValidator<AppRole>>()
             .AddEntityFrameworkStores<DataContext>()
             .AddDefaultTokenProviders();

            // Token configuration
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("AppSettings:Token").Value));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opt =>
                {
                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = key,
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    };
                });

            // Role configuration
            services.AddAuthorization(opt =>
            {
                opt.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin")); // Admin only
                opt.AddPolicy("RequireModeratorRole", policy => policy.RequireRole("Admin", "Moderator")); // Admin or Moderator
            });

            return services;
        }
    }
}
