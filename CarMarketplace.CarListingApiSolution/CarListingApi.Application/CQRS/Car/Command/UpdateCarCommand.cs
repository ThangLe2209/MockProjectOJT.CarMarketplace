using CarListingApi.Application.DTOs;
using MediatR;

namespace CarListingApi.Application.CQRS.Car.Command
{
    public record UpdateCarCommand(int carId, CarInputDto Car) : IRequest<Unit>;
}
