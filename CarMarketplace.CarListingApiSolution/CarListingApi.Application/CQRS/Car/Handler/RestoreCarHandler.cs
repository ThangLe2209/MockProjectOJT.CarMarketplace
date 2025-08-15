using AutoMapper;
using CarListingApi.Application.CQRS.Car.Command;
using CarListingApi.Application.DTOs;
using CarListingApi.Domain.Entities;
using CarListingApi.Domain.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CarListingApi.Application.CQRS.Car.Handler
{
    public class RestoreCarHandler : IRequestHandler<RestoreCarCommand, Unit>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public RestoreCarHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(RestoreCarCommand request, CancellationToken cancellationToken)
        {
            var collection = _unitOfWork.Cars.GetAll().IgnoreQueryFilters();
            var currentCar = await collection.FirstOrDefaultAsync(c => c.Id == request.Id);
            
            if (currentCar == null)
            {
                throw new Exception($"Car not found.");
            }

            currentCar.IsDeleted = false;
            await _unitOfWork.CompleteAsync();

            return Unit.Value;
        }
    }
}
