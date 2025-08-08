using AuthenticationApi.Domain.Entities;
using AuthenticationApi.Infrastructure.Data;
using CarMarketplace.Contracts.Events;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AuthenticationApi.Infrastructure.Services
{
    public class OutboxPublisherService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<OutboxPublisherService> _logger;

        public OutboxPublisherService(IServiceProvider serviceProvider, ILogger<OutboxPublisherService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _serviceProvider.CreateScope();
                    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                    var publisher = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();

                    var unpublishedEvents = await dbContext.OutboxEvents
                        .Where(e => !e.IsPublished)
                        .OrderBy(e => e.CreatedDate)
                        .Take(10)
                        .ToListAsync(stoppingToken);

                    foreach (var outboxEvent in unpublishedEvents)
                    {
                        try
                        {
                            await PublishEvent(outboxEvent, publisher, stoppingToken);

                            outboxEvent.IsPublished = true;
                            dbContext.Update(outboxEvent);
                            await dbContext.SaveChangesAsync(stoppingToken);

                            _logger.LogInformation("Published outbox event {EventType} with ID {EventId}", outboxEvent.Type, outboxEvent.Id);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Failed to publish outbox event {EventType} with ID {EventId}", outboxEvent.Type, outboxEvent.Id);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error in OutboxPublisherService");
                }

                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }
        }

        private async Task PublishEvent(OutboxEvent outboxEvent, IPublishEndpoint publisher, CancellationToken cancellationToken)
        {
            switch (outboxEvent.Type)
            {
                case nameof(UserRegisteredEvent):
                    var userEvent = JsonConvert.DeserializeObject<UserRegisteredEvent>(outboxEvent.Payload);
                    if (userEvent != null)
                    {
                        await publisher.Publish(userEvent, context =>
                        {
                            if (!string.IsNullOrEmpty(outboxEvent.Headers))
                            {
                                var headers = JsonConvert.DeserializeObject<Dictionary<string, object>>(outboxEvent.Headers);
                                if (headers != null)
                                {
                                    foreach (var header in headers)
                                    {
                                        context.Headers.Set(header.Key, header.Value);
                                    }
                                }
                            }
                        }, cancellationToken);
                    }
                    break;
                default:
                    _logger.LogWarning("Unknown event type: {EventType}", outboxEvent.Type);
                    break;
            }
        }
    }
}