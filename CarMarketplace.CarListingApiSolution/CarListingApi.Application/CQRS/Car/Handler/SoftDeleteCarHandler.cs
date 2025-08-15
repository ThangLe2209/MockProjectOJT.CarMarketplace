using AutoMapper;
using CarListingApi.Application.CQRS.Car.Command;
using CarListingApi.Application.DTOs;
using CarListingApi.Domain.Entities;
using CarListingApi.Domain.Interfaces;
using FluentValidation;
using MediatR;

namespace CarListingApi.Application.CQRS.Car.Handler
{
    public class SoftDeleteCarHandler : IRequestHandler<SoftDeleteCarCommand, Unit>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public SoftDeleteCarHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(SoftDeleteCarCommand request, CancellationToken cancellationToken)
        {
            var currentCar = await _unitOfWork.Cars.GetByIdAsync(request.Id);
            if (currentCar == null)
            {
                throw new Exception($"Car not found.");
            }

            currentCar.IsDeleted = true;
            await _unitOfWork.CompleteAsync();

            return Unit.Value;
        }
    }
}
