using AutoMapper;
using CarListingApi.Application.CQRS.Car.Command;
using CarListingApi.Application.DTOs;
using CarListingApi.Domain.Entities;
using CarListingApi.Domain.Interfaces;
using FluentValidation;
using MediatR;

namespace CarListingApi.Application.CQRS.Car.Handler
{
    public class CreateCarHandler(
        IValidator<CarInputDto> validator,
        IMapper _mapper,
        IUnitOfWork _unitOfWork) : IRequestHandler<CreateCarCommand, CarListingDto>
    {
        public async Task<CarListingDto> Handle(CreateCarCommand request, CancellationToken cancellationToken)
        {
            var validate = await validator.ValidateAsync(request.CarInput);
            if (!validate.IsValid)
                throw new Exception(string.Join("; ", validate.Errors.Select(error => error.ErrorMessage)));

            var checkCarTitle = await _unitOfWork.Cars.ExistsAsync(c => c.Title.ToLower().Equals(request.CarInput.Title.ToLower()));
            if (checkCarTitle) throw new Exception("Car title already exists");
            var finalCar = _mapper.Map<CarListing>(request.CarInput);
            await _unitOfWork.Cars.AddAsync(finalCar);
            await _unitOfWork.CompleteAsync();
            return _mapper.Map<CarListingDto>(finalCar);
        }
    }
}