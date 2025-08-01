using MediatR;

namespace CarListingApi.Application.CQRS.Car.Command
{
    public record DeleteCarCommand(int Id) : IRequest<Unit>;
}
