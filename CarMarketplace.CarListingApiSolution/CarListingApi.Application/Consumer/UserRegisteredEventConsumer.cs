using CarMarketplace.Contracts.Events;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using CarListingApi.Domain.Interfaces;
using CarListingApi.Domain.Entities;
using System;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CarListingApi.Application.Consumer
{
    public class UserRegisteredEventConsumer : IConsumer<UserRegisteredEvent>
    {
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UserRegisteredEventConsumer> _logger;

        public UserRegisteredEventConsumer(
            IConfiguration configuration,
            IUnitOfWork unitOfWork,
            ILogger<UserRegisteredEventConsumer> logger)
        {
            _configuration = configuration;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<UserRegisteredEvent> context)
        {
            var eventId = context.MessageId ?? Guid.NewGuid();

            try
            {
                if (await IsEventProcessedAsync(eventId))
                {
                    _logger.LogInformation("Event {EventId} already processed. Skipping.", eventId);
                    return;
                }

                if (!IsValidSecret(context))
                {
                    _logger.LogWarning("Invalid message secret for event {EventId}.", eventId);
                    throw new UnauthorizedAccessException("Invalid message secret!");
                }

                await HandleUserRegisteredEventAsync(context.Message);

                await MarkEventAsProcessedAsync(eventId, nameof(UserRegisteredEvent), context.Message);

                _logger.LogInformation("Successfully processed UserRegisteredEvent: {UserId}, {Email}, EventId: {EventId}",
                    context.Message.UserId, context.Message.Email, eventId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to process UserRegisteredEvent: {UserId}, {Email}, EventId: {EventId}",
                    context.Message.UserId, context.Message.Email, eventId);
                throw; // Optionally rethrow to let MassTransit handle retries/DLQ
            }
        }

        private async Task<bool> IsEventProcessedAsync(Guid eventId)
        {
            return await _unitOfWork.ProcessedEvents.ExistsAsync(p => p.Id == eventId);
        }

        private bool IsValidSecret(ConsumeContext<UserRegisteredEvent> context)
        {
            var expectedSecret = _configuration["RabbitMq:SharedSecret"];
            var receivedSecret = context.Headers.Get<string>("X-Service-Secret");
            return receivedSecret == expectedSecret;
        }

        private async Task HandleUserRegisteredEventAsync(UserRegisteredEvent message)
        {
            // TODO: Add your actual event handling logic here
            // _logger.LogDebug("Handling UserRegisteredEvent: {UserId}, {Email}", message.UserId, message.Email);
            _logger.LogInformation("Handling UserRegisteredEvent: {UserId}, {Email}", message.UserId, message.Email);
            await Task.CompletedTask;
        }

        private async Task MarkEventAsProcessedAsync(Guid eventId, string eventType, UserRegisteredEvent payload)
        {
            await _unitOfWork.ProcessedEvents.AddAsync(new ProcessedEvent
            {
                Id = eventId,
                EventType = eventType,
                Payload = JsonConvert.SerializeObject(payload),
                ProcessedAt = DateTime.Now // or DateTime.UtcNow
            });
            await _unitOfWork.CompleteAsync();
        }
    }
}