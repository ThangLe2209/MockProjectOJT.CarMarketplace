using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using OrderApi.Application.CQRS.Car.Command;
using OrderApi.Application.DTOs;
using OrderApi.Domain.Entities;
using OrderApi.Domain.Interfaces;

namespace OrderApi.Application.CQRS.Car.Handler
{
    public class UpdateOrderStatusHandler(
        IUnitOfWork _unitOfWork, ILogger<UpdateOrderStatusHandler> _logger) : IRequestHandler<UpdateOrderStatusCommand, Unit>
    {
        public async Task<Unit> Handle(UpdateOrderStatusCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Update order status with OrderId {OrderId}", request.OrderId);
            if (String.IsNullOrEmpty(request.Status)) throw new Exception("Status can not be empty");

            Order? currentOrder = await _unitOfWork.Orders.GetByIdAsync(request.OrderId) ?? throw new Exception("Order not found");
            currentOrder.Status = request.Status;
            await _unitOfWork.CompleteAsync();

            _logger.LogInformation("Order status updated successfully with ID {OrderId}", currentOrder.Id);
            return Unit.Value;
        }
    }
}