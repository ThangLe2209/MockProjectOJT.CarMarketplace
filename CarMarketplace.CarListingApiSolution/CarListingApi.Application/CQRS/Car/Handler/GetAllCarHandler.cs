using AutoMapper;
using CarListingApi.Application.CQRS.Car.Command;
using CarListingApi.Application.CQRS.Car.Query;
using CarListingApi.Application.DTOs;
using CarListingApi.Domain.Entities;
using CarListingApi.Domain.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CarListingApi.Application.CQRS.Car.Handler
{
    public class GetAllCarHandler : IRequestHandler<GetAllCarQuery, (IEnumerable<CarListingDto>, PaginationMetadata)>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public GetAllCarHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<(IEnumerable<CarListingDto>, PaginationMetadata)> Handle(GetAllCarQuery request, CancellationToken cancellationToken)
        {
            int maxCarsPageSize = 10;
            var pageSize = request.pageSize;
            if (pageSize > maxCarsPageSize)
            {
                pageSize = maxCarsPageSize;
            }

            var collection = _unitOfWork.Cars.GetAll();

            var searchTerm = request.searchTerm;
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = searchTerm.Trim();
                collection = collection.Where(t => t.Title.ToLower().Contains(searchTerm.ToLower()));
            }

            // Sorting logic
            switch (request.sort?.ToLower())
            {
                case "price_asc":
                    collection = collection.OrderBy(t => t.Price);
                    break;
                case "price_desc":
                    collection = collection.OrderByDescending(t => t.Price);
                    break;
                case "year_asc":
                    collection = collection.OrderBy(t => t.Year);
                    break;
                case "year_desc":
                    collection = collection.OrderByDescending(t => t.Year);
                    break;
                default:
                    collection = collection.OrderByDescending(t => t.UpdatedDate);
                    break;
            }

            var totalItemCount = await collection.CountAsync();

            var pageNumber = request.pageNumber;
            var paginationMetadata = new PaginationMetadata(totalItemCount, pageSize, pageNumber);

            var collectionToReturn = await collection
                            .Skip(pageSize * (pageNumber - 1))
                            .Take(pageSize)
                            .ToListAsync();

            return (_mapper.Map<IEnumerable<CarListingDto>>(collectionToReturn), paginationMetadata);
        }
    }
}
