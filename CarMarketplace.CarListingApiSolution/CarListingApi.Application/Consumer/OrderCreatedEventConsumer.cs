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
    public class OrderCreatedEventConsumer : IConsumer<OrderCreatedEvent>
    {
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<OrderCreatedEventConsumer> _logger;

        public OrderCreatedEventConsumer(
            IConfiguration configuration,
            IUnitOfWork unitOfWork,
            ILogger<OrderCreatedEventConsumer> logger)
        {
            _configuration = configuration;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
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

                await HandleOrderCreatedEventAsync(context.Message);

                await MarkEventAsProcessedAsync(eventId, nameof(OrderCreatedEvent), context.Message);

                _logger.LogInformation("Successfully processed OrderCreatedEvent: {OrderId}, {BuyerId}, EventId: {EventId}",
                    context.Message.OrderId, context.Message.BuyerId, eventId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to process OrderCreatedEvent: {OrderId}, {BuyerId}, EventId: {EventId}",
                    context.Message.OrderId, context.Message.BuyerId, eventId);
                throw; // Optionally rethrow to let MassTransit handle retries/DLQ
            }
        }

        private async Task<bool> IsEventProcessedAsync(Guid eventId)
        {
            return await _unitOfWork.ProcessedEvents.ExistsAsync(p => p.Id == eventId);
        }

        private bool IsValidSecret(ConsumeContext<OrderCreatedEvent> context)
        {
            var expectedSecret = _configuration["RabbitMq:SharedSecret"];
            var receivedSecret = context.Headers.Get<string>("X-Service-Secret");
            return receivedSecret == expectedSecret;
        }

        private async Task HandleOrderCreatedEventAsync(OrderCreatedEvent message)
        {
            _logger.LogInformation("Handling OrderCreatedEvent: {OrderId}, {BuyerId}", message.OrderId, message.BuyerId);
            foreach (var item in message.Items)
            {
                var car = await _unitOfWork.Cars.GetByIdAsync(item.CarId);
                if (car != null)
                {
                    car.Quantity -= item.Quantity;
                    car.Status = car.Quantity > 0 ? "Available" : "Sold out";
                }
                await _unitOfWork.CompleteAsync();
            }
            await Task.CompletedTask;
        }

        private async Task MarkEventAsProcessedAsync(Guid eventId, string eventType, OrderCreatedEvent payload)
        {
            await _unitOfWork.ProcessedEvents.AddAsync(new ProcessedEvent
            {
                Id = eventId,
                EventType = eventType,
                Payload = JsonConvert.SerializeObject(payload),
                ProcessedAt = DateTime.UtcNow // or DateTime.UtcNow
            });
            await _unitOfWork.CompleteAsync();
        }
    }
}