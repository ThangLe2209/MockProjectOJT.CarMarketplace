using AutoMapper;
using CarListingApi.Application.CQRS.Car.Command;
using CarListingApi.Application.CQRS.Car.Query;
using CarListingApi.Application.DTOs;
using CarListingApi.Domain.Entities;
using CarListingApi.Domain.Interfaces;
using FluentValidation;
using MediatR;

namespace CarListingApi.Application.CQRS.Car.Handler
{
    public class GetCarHandler : IRequestHandler<GetCarQuery, CarListingDto>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public GetCarHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<CarListingDto> Handle(GetCarQuery request, CancellationToken cancellationToken)
        {
            var car = await _unitOfWork.Cars.GetByIdAsync(request.Id);

            return _mapper.Map<CarListingDto>(car);
        }
    }
}
