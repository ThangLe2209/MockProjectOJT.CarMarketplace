using AutoMapper;
using MediatR;
using OrderApi.Application.CQRS.Car.Command;
using OrderApi.Domain.Interfaces;

namespace OrderApi.Application.CQRS.Car.Handler
{
    public class SoftDeleteOrderHandler : IRequestHandler<SoftDeleteOrderCommand, Unit>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public SoftDeleteOrderHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(SoftDeleteOrderCommand request, CancellationToken cancellationToken)
        {
            var currentOrder = await _unitOfWork.Orders.GetByIdAsync(request.Id) ?? throw new Exception($"Order not found.");

            currentOrder.IsDeleted = true;
            await _unitOfWork.CompleteAsync();

            return Unit.Value;
        }
    }
}
