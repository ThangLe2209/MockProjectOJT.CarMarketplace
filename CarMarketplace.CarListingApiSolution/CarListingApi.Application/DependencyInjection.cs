using CarListingApi.Application.Consumer;
using CarListingApi.Application.CQRS.Car.Handler;
using CarListingApi.Application.CQRS.Car.Validation;
using FluentValidation;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarListingApi.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration config)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddMassTransit(x =>
            {
                x.AddConsumer<UserRegisteredEventConsumer>();
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host("localhost", h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });
                    cfg.ReceiveEndpoint("user-registered-queue", e =>
                    {
                        e.ConfigureConsumer<UserRegisteredEventConsumer>(context);
                    });
                });
            });

            //services.AddScoped<IEventBus, MassTransitEventBus>();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateCarHandler).Assembly));
            services.AddValidatorsFromAssemblyContaining<CarInputValidator>();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            return services;
        }
    }
}
