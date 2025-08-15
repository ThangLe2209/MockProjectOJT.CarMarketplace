using MediatR;
using OrderApi.Application.DTOs;

namespace OrderApi.Application.CQRS.Car.Query
{
    public record GetOrderByIdQuery(int Id) : IRequest<OrderDto>;
}
