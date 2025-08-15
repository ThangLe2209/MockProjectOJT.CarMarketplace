using AutoMapper;
using CarListingApi.Application.CQRS.Car.Query;
using CarListingApi.Application.DTOs;
using CarListingApi.Application.Services;
using CarListingApi.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarListingApi.Application.CQRS.Car.Handler
{
    // Example in a CQRS handler
    public class GetCarWithSellerHandler : IRequestHandler<GetCarWithSellerQuery, CarWithSellerDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public GetCarWithSellerHandler(IUnitOfWork unitOfWork, IUserService userService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _userService = userService;
            _mapper = mapper;
        }

        public async Task<CarWithSellerDto> Handle(GetCarWithSellerQuery request, CancellationToken cancellationToken)
        {
            var car = await _unitOfWork.Cars.GetByIdAsync(request.CarId);
            if (car == null) return null!;

            var seller = await _userService.GetUserAsync(car.SellerId);

            return new CarWithSellerDto
            {
                Id = car.Id,
                Title = car.Title,
                Description = car.Description,
                Price = car.Price,
                Make = car.Make,
                Model = car.Model,
                Year = car.Year,
                Mileage = car.Mileage,
                Color = car.Color,
                Image = car.Image,
                CreatedDate = car.CreatedDate,
                UpdatedDate = car.UpdatedDate,
                Status = car.Status,
                Quantity = car.Quantity,
                Seller = seller
            };
        }
    }
}
