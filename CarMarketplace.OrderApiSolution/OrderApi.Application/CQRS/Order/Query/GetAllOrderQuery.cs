using MediatR;
using OrderApi.Application.DTOs;

namespace OrderApi.Application.CQRS.Car.Query
{
    public record GetAllOrderQuery(string? searchTerm = "", int pageNumber = 1, int pageSize = 10, string? sort = "year_asc") : IRequest<(IEnumerable<OrderDto>, PaginationMetadata)>;
}
