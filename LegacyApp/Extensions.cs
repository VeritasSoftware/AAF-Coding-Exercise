using LegacyApp.Abstractions;
using LegacyApp.Repository;
using LegacyApp.Services;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace LegacyApp
{
    public static class Extensions
    {
        public static IServiceCollection AddLegacyApp(this IServiceCollection services, Action<Settings> configure = null)
        {
            var settings = new Settings();

            services.AddScoped<IUserCreditService>(sp =>
            {
                var client = new UserCreditServiceClient();
                settings.Client = client;
                configure?.Invoke(settings);
                return client;
            });
            services.AddScoped<IUserRepository, UserRepository>();  
            services.AddScoped<IClientRepository, ClientRepository>();
            services.AddScoped<IUserService, UserService>();
            return services;
        }
    }
}
