using CarMarketplace.Contracts.Logging;
using AuthenticationApi.Infrastructure.Services;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace AuthenticationApi.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            services.AddMassTransit(x =>
            {
                // Add the EF Core outbox
                //x.AddEntityFrameworkOutbox<AppDbContext>(o =>
                //{
                //    o.UseSqlServer();
                //    o.QueryDelay = TimeSpan.FromSeconds(10); // How often to check for new outbox messages
                //    //o.DisableDeliveryService = false; // Enable background delivery
                //});

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

            services.AddSharedSerilog(config, "AuthenticationServiceLog");
            return services;
        }
    }
}
