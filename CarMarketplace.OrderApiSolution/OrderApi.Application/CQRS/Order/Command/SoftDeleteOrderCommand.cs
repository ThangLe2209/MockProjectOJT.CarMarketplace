using MediatR;

namespace OrderApi.Application.CQRS.Car.Command
{
    public record SoftDeleteOrderCommand(int Id) : IRequest<Unit>;
}
