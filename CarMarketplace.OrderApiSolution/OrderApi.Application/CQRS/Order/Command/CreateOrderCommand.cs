using MediatR;
using OrderApi.Application.DTOs;

namespace OrderApi.Application.CQRS.Car.Command
{
    public record CreateOrderCommand(OrderInputDto OrderInput) : IRequest<OrderDto>;
}
