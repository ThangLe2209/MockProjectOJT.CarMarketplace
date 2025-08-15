using MediatR;
using Microsoft.AspNetCore.Mvc;
using OrderApi.Application.CQRS.Car.Command;
using OrderApi.Application.CQRS.Car.Query;
using OrderApi.Application.DTOs;
using System.Text.Json;

namespace OrderApi.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : Controller
    {
        private readonly ISender _sender;
        private readonly ILogger<OrdersController> _logger;
        public OrdersController(ISender sender, ILogger<OrdersController> logger)
        {
            _sender = sender ?? throw new ArgumentNullException(nameof(sender));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        //[Authorize(Roles = "Admin")]
        //[Authorize(Policy = "CheckAdminPolicy")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrders(string? searchTerm = "", int pageNumber = 1, int pageSize = 10, string? sort = "year_asc")
        {
            var (orderEntities, paginationMetadata) = await _sender.Send(new GetAllOrderQuery(searchTerm, pageNumber, pageSize, sort));

            Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(paginationMetadata));
            return Ok(new SuccessResponse<IEnumerable<OrderDto>>(orderEntities, HttpContext.Request.Path));
        }

        [HttpGet("{orderId}", Name = "GetOrderById")]
        public async Task<IActionResult> GetOrderByIdAsync(int orderId)
        {
            _logger.LogInformation("Fetching order with ID {OrderId}", orderId);

            try
            {
                var order = await _sender.Send(new GetOrderByIdQuery(orderId));
                if (order == null)
                {
                    _logger.LogWarning("Order with ID {OrderId} not found", orderId);
                    return NotFound(new BadRequestResponse(HttpContext.Request.Path, "Order not existed"));
                }

                _logger.LogInformation("Successfully fetched order with ID {OrderId}", orderId);
                return Ok(new SuccessResponse<OrderDto>(order, HttpContext.Request.Path));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching order with ID {OrderId}", orderId);
                throw;
            }
        }

        [HttpGet("getByBuyerId/{buyerId}")]
        public async Task<IActionResult> GetOrdersByBuyerIdAsync(int buyerId)
        {
            var cars = await _sender.Send(new GetOrdersByBuyerIdQuery(buyerId));
            if (cars == null)
            {
                return NotFound(new BadRequestResponse(HttpContext.Request.Path, "Orders not existed"));
            }

            return Ok(new SuccessResponse<IEnumerable<OrderDto>>(cars, HttpContext.Request.Path));
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrderAsync(OrderInputDto order)
        {
            //var assignerId = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var createdOrderToReturn = await _sender.Send(new CreateOrderCommand(order));
            return CreatedAtRoute("GetOrderById",
                new
                {
                    orderId = createdOrderToReturn.Id,
                }
                , new SuccessResponse<OrderDto>(createdOrderToReturn, HttpContext.Request.Path, "Order created successfully"));
        }

        [HttpPut("{orderId}")]
        public async Task<IActionResult> UpdateOrderStatusAsync(int orderId, string status)
        {
            await _sender.Send(new UpdateOrderStatusCommand(orderId, status));
            return NoContent();
        }

        [HttpDelete("soft/{orderId}")]
        public async Task<ActionResult> DeleteOrderAsync(int orderId)
        {
            await _sender.Send(new SoftDeleteOrderCommand(orderId));
            return NoContent();
        }
    }
}
