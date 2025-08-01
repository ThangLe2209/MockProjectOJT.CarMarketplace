using CarListingApi.Application.DTOs;
using MediatR;

namespace CarListingApi.Application.CQRS.Car.Query
{
    public record GetCarQuery(int Id) : IRequest<CarListingDto>;
}
