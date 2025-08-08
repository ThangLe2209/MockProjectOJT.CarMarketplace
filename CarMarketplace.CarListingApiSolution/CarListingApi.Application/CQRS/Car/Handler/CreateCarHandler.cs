using AutoMapper;
using CarListingApi.Application.CQRS.Car.Command;
using CarListingApi.Application.DTOs;
using CarListingApi.Domain.Entities;
using CarListingApi.Domain.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CarListingApi.Application.CQRS.Car.Handler
{
    public class CreateCarHandler(
        IValidator<CarInputDto> validator,
        IMapper _mapper,
        IUnitOfWork _unitOfWork, ILogger<CreateCarHandler> _logger) : IRequestHandler<CreateCarCommand, CarListingDto>
    {
        public async Task<CarListingDto> Handle(CreateCarCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Creating car with title {Title}", request.CarInput.Title);
            var validate = await validator.ValidateAsync(request.CarInput);
            if (!validate.IsValid)
            {
                _logger.LogWarning("Validation failed for car creation: {Errors}", string.Join("; ", validate.Errors.Select(e => e.ErrorMessage)));
                throw new Exception(string.Join("; ", validate.Errors.Select(error => error.ErrorMessage)));
            }

            var checkCarTitle = await _unitOfWork.Cars.ExistsAsync(c => c.Title.ToLower().Equals(request.CarInput.Title.ToLower()));
            if (checkCarTitle)
            {
                _logger.LogWarning("Car title '{Title}' already exists", request.CarInput.Title);
                throw new Exception("Car title already exists");
            }
            var finalCar = _mapper.Map<CarListing>(request.CarInput);
            await _unitOfWork.Cars.AddAsync(finalCar);
            await _unitOfWork.CompleteAsync();

            _logger.LogInformation("Car created successfully with ID {CarId}", finalCar.Id);
            return _mapper.Map<CarListingDto>(finalCar);
        }
    }
}