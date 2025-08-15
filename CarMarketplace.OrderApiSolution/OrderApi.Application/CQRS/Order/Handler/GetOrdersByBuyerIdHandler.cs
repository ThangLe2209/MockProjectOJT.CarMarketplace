using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderApi.Application.CQRS.Car.Query;
using OrderApi.Application.DTOs;
using OrderApi.Domain.Interfaces;

namespace OrderApi.Application.CQRS.Car.Handler
{
    public class GetOrdersByBuyerIdHandler : IRequestHandler<GetOrdersByBuyerIdQuery, IEnumerable<OrderDto>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public GetOrdersByBuyerIdHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<OrderDto>> Handle(GetOrdersByBuyerIdQuery request, CancellationToken cancellationToken)
        {
            var order = await _unitOfWork.Orders.GetAll().AsNoTracking()
                .Include(o => o.Items).Where(o => o.BuyerId.Equals(request.BuyerId)).ToListAsync();

            return _mapper.Map<IEnumerable<OrderDto>>(order);
        }
    }
}
