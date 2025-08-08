using CarListingApi.Application.DTOs;
using MediatR;

namespace CarListingApi.Application.CQRS.Car.Query
{
    public record GetAllCarQuery(string? searchTerm = "", int pageNumber = 1, int pageSize = 10, string? sort = "price_asc") : IRequest<(IEnumerable<CarListingDto>, PaginationMetadata)>;
}
