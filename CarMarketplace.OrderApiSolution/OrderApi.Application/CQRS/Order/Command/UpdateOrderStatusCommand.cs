using MediatR;
using OrderApi.Application.DTOs;

namespace OrderApi.Application.CQRS.Car.Command
{
    public record UpdateOrderStatusCommand(int OrderId, string Status) : IRequest<Unit>;
}
