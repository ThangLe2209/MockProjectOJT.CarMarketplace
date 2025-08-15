using MediatR;

namespace CarListingApi.Application.CQRS.Car.Command
{
    public record SoftDeleteCarCommand(int Id) : IRequest<Unit>;
}
