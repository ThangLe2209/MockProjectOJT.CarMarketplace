using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OrderApi.Application.CQRS.Car.Query;
using OrderApi.Application.DTOs;
using OrderApi.Domain.Interfaces;

namespace OrderApi.Application.CQRS.Car.Handler
{
    public class GetAllOrderHandler : IRequestHandler<GetAllOrderQuery, (IEnumerable<OrderDto>, PaginationMetadata)>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GetAllOrderHandler> _logger;

        public GetAllOrderHandler(IMapper mapper, IUnitOfWork unitOfWork, ILogger<GetAllOrderHandler> logger)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<(IEnumerable<OrderDto>, PaginationMetadata)> Handle(GetAllOrderQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Start: fetch get all orders");

            int maxOrdersPageSize = 5000;
            var pageSize = request.pageSize;
            if (pageSize > maxOrdersPageSize)
            {
                pageSize = maxOrdersPageSize;
            }

            var collection = _unitOfWork.Orders.GetAll();

            var searchTerm = request.searchTerm;
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = searchTerm.Trim();
                collection = collection.Where(t => t.BuyerId.ToString().ToLower().Contains(searchTerm.ToLower()));
            }


            // Sorting logic
            //switch (request.sort?.ToLower())
            //{
            //    case "year_asc":
            //        collection = collection.OrderBy(t => t.Year);
            //        break;
            //    case "year_desc":
            //        collection = collection.OrderByDescending(t => t.Year);
            //        break;
            //    default:
            //        collection = collection.OrderByDescending(t => t.UpdatedDate);
            //        break;
            //}

            var totalItemCount = await collection.CountAsync();

            var pageNumber = request.pageNumber;
            var paginationMetadata = new PaginationMetadata(totalItemCount, pageSize, pageNumber);

            var collectionToReturn = await collection
                            .Include(o => o.Items)
                            .Skip(pageSize * (pageNumber - 1))
                            .Take(pageSize)
                            .ToListAsync();

            _logger.LogInformation("End: fetch get all orders");
            return (_mapper.Map<IEnumerable<OrderDto>>(collectionToReturn), paginationMetadata);
        }
    }
}
