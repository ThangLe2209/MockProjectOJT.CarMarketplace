using AuthenticationApi.Application.Interfaces;
using AuthenticationApi.Application.Services;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationApi.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration config)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


            services.AddMassTransit(x =>
            {
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host("localhost", h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });
                });
            });

            services.AddScoped<IAuthenticationService, AuthenticationService>();
            return services;
        }
    }
}
