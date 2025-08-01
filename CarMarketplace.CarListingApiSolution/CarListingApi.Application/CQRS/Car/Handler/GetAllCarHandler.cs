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
    public class GetAllCarHandler : IRequestHandler<GetAllCarQuery, IEnumerable<CarListingDto>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public GetAllCarHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<CarListingDto>> Handle(GetAllCarQuery request, CancellationToken cancellationToken)
        {
            var cars = await _unitOfWork.Cars.GetAllAsync();

            return _mapper.Map<IEnumerable<CarListingDto>>(cars);
        }
    }
}
