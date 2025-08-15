using MediatR;

namespace CarListingApi.Application.CQRS.Car.Command
{
    public record RestoreCarCommand(int Id) : IRequest<Unit>;
}
