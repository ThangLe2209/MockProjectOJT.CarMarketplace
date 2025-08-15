using CarMarketplace.Contracts.Logging;
using CarListingApi.Domain.Interfaces;
using CarListingApi.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MassTransit;
using CarListingApi.Application.Consumer;


namespace CarListingApi.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            services.AddScoped<ICarRepository, CarRepository>();

            services.AddMassTransit(x =>
            {
                x.AddConsumer<UserRegisteredEventConsumer>();
                x.AddConsumer<OrderCreatedEventConsumer>();
                x.UsingRabbitMq((context, cfg) =>
                {
                    var configuration = context.GetRequiredService<IConfiguration>();
                    var rabbitMqSection = configuration.GetSection("RabbitMq");
                    var host = rabbitMqSection["Host"] ?? "localhost";
                    var username = rabbitMqSection["Username"] ?? "guest";
                    var password = rabbitMqSection["Password"] ?? "guest";

                    cfg.Host(host, h =>
                    {
                        h.Username(username);
                        h.Password(password);
                    });

                    cfg.ReceiveEndpoint("user-registered-queue", e =>
                    {
                        e.ConfigureConsumer<UserRegisteredEventConsumer>(context);
                    });
                    cfg.ReceiveEndpoint("order-created-queue", e =>
                    {
                        e.ConfigureConsumer<OrderCreatedEventConsumer>(context);
                    });
                });
            });
            //services.AddScoped<IEventBus, MassTransitEventBus>();

            services.AddSharedSerilog(config, "CarServiceLog");
            return services;
        }
    }
}
