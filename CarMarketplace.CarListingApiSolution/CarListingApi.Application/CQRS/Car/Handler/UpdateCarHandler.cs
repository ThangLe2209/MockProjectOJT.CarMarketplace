using AutoMapper;
using CarListingApi.Application.CQRS.Car.Command;
using CarListingApi.Application.DTOs;
using CarListingApi.Domain.Entities;
using CarListingApi.Domain.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CarListingApi.Application.CQRS.Car.Handler
{
    public class UpdateCarHandler(
        IValidator<CarInputDto> validator,
        IMapper _mapper,
        IUnitOfWork _unitOfWork) : IRequestHandler<UpdateCarCommand, Unit>
    {
        public async Task<Unit> Handle(UpdateCarCommand request, CancellationToken cancellationToken)
        {
            var validate = await validator.ValidateAsync(request.Car);
            if (!validate.IsValid)
                throw new Exception(string.Join("; ", validate.Errors.Select(error => error.ErrorMessage)));

            CarListing? currentCar = await _unitOfWork.Cars.GetByIdAsync(request.carId);
            if (currentCar == null) throw new Exception("Car not found");

            _mapper.Map(request.Car, currentCar); // source, dest => use mapper like this will override data from source to dest
            await _unitOfWork.CompleteAsync();

            return Unit.Value;
        }
    }
}
