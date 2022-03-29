using Application.CurrentUserAccessor;
using Application.Email;
using Application.Interfaces;
using Application.Mapping;
using Application.Photos;
using Application.WorkoutEvents;
using BROwser_API.Interfaces;
using BROwser_API.Services;
using Database;
using MediatR;
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
            // Database connections

            // MS SQL Connection
            services.AddDbContext<DataContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            // PostgreSQL Connection
            //services.AddDbContext<DataContext>(options =>
            //{
            //    var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            //    string connStr;

            //    if (env == "Development")
            //    {
            //        // Use connection string from file.
            //        connStr = configuration.GetConnectionString("PostgresqlConnection");
            //    }
            //    else
            //    {
            //        // Use connection string provided at runtime by Heroku.
            //        var connUrl = Environment.GetEnvironmentVariable("DATABASE_URL");

            //        // Parse connection URL to connection string for Npgsql
            //        connUrl = connUrl.Replace("postgres://", string.Empty);
            //        var pgUserPass = connUrl.Split("@")[0];
            //        var pgHostPortDb = connUrl.Split("@")[1];
            //        var pgHostPort = pgHostPortDb.Split("/")[0];
            //        var pgDb = pgHostPortDb.Split("/")[1];
            //        var pgUser = pgUserPass.Split(":")[0];
            //        var pgPass = pgUserPass.Split(":")[1];
            //        var pgHost = pgHostPort.Split(":")[0];
            //        var pgPort = pgHostPort.Split(":")[1];

            //        connStr = $"Server={pgHost};Port={pgPort};User Id={pgUser};Password={pgPass};Database={pgDb}; SSL Mode=Require; Trust Server Certificate=true";
            //    }

            //    options.UseNpgsql(connStr);
            //});

            // Services
            services.AddMediatR(typeof(EventList.Handler).Assembly);
            services.AddAutoMapper(typeof(MappingProfiles).Assembly);
            services.Configure<CloudinarySettings>(configuration.GetSection("Cloudinary"));
            services.AddScoped<UserActivityLogger>();
            services.AddScoped<EmailSender>();

            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IPhotoAccessor, PhotoAccessor>();
            services.AddScoped<IUserAccessor, UserAccessor>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "BROwser_API", Version = "v1" });
            });

            return services;
        }
    }
}
