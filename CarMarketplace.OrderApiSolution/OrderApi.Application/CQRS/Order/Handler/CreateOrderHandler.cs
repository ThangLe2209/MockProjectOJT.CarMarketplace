using AutoMapper;
using CarMarketplace.Contracts.Events;
using FluentValidation;
using MassTransit.Transports;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OrderApi.Application.CQRS.Car.Command;
using OrderApi.Application.DTOs;
using OrderApi.Domain.Entities;
using OrderApi.Domain.Interfaces;

namespace OrderApi.Application.CQRS.Car.Handler
{
    public class CreateOrderHandler(
        IValidator<OrderInputDto> validator,
        IMapper _mapper,
        IUnitOfWork _unitOfWork, ILogger<CreateOrderHandler> _logger, IConfiguration _configuration) : IRequestHandler<CreateOrderCommand, OrderDto>
    {
        public async Task<OrderDto> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Creating order with BuyerId {BuyerId}", request.OrderInput.BuyerId);
            var validate = await validator.ValidateAsync(request.OrderInput);
            if (!validate.IsValid)
            {
                _logger.LogWarning("Validation failed for order creation: {Errors}", string.Join("; ", validate.Errors.Select(e => e.ErrorMessage)));
                throw new Exception(string.Join("; ", validate.Errors.Select(error => error.ErrorMessage)));
            }

            var finalOrder = _mapper.Map<Order>(request.OrderInput);
            finalOrder.TotalPrice = finalOrder.Items.Sum(i => i.Price * i.Quantity);
            await _unitOfWork.Orders.AddAsync(finalOrder);
            await _unitOfWork.CompleteAsync();

            // Prepare event and headers
            var orderCreatedEvent = new OrderCreatedEvent
            {
                OrderId = finalOrder.Id,
                BuyerId = finalOrder.BuyerId,
                Items = finalOrder.Items.Select(i => new CarMarketplace.Contracts.Events.OrderItemDto
                {
                    CarId = i.CarId,
                    Quantity = i.Quantity,
                    Price = i.Price
                }).ToList()
            };
            var headers = new Dictionary<string, object>
            {
                { "X-Service-Secret", _configuration["RabbitMq:SharedSecret"] ?? "" }
            };

            // Always store in outbox for background publishing
            await StoreOutboxEventAsync(nameof(OrderCreatedEvent), orderCreatedEvent, headers);

            _logger.LogInformation("Order created successfully with ID {OrderId}", finalOrder.Id);
            return _mapper.Map<OrderDto>(finalOrder);
        }

        private async Task StoreOutboxEventAsync(string eventType, object eventPayload, Dictionary<string, object> headers)
        {
            var outboxEvent = new OutboxEvent
            {
                Type = eventType,
                Payload = JsonConvert.SerializeObject(eventPayload),
                Headers = JsonConvert.SerializeObject(headers),
                IsPublished = false
            };
            await _unitOfWork.OutboxEvents.AddAsync(outboxEvent);
            await _unitOfWork.CompleteAsync();
        }
    }
}