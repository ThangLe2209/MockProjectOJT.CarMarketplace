using MediatR;
using OrderApi.Application.DTOs;

namespace OrderApi.Application.CQRS.Car.Query
{
    public record GetOrdersByBuyerIdQuery(int BuyerId) : IRequest<IEnumerable<OrderDto>>;
}
