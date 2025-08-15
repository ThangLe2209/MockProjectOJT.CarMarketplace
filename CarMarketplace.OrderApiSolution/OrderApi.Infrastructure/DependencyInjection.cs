using CarMarketplace.Contracts.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MassTransit;
using OrderApi.Infrastructure.Services;


namespace OrderApi.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            services.AddMassTransit(x =>
            {
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
                });
            });

            services.AddHostedService<OutboxPublisherService>();

            services.AddSharedSerilog(config, "OrderServiceLog");
            return services;
        }
    }
}
