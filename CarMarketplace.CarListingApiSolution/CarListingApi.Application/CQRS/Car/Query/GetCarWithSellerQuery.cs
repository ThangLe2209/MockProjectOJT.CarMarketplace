using CarListingApi.Application.DTOs;
using MediatR;

namespace CarListingApi.Application.CQRS.Car.Query
{
    public record GetCarWithSellerQuery(int CarId) : IRequest<CarWithSellerDto>;
}
