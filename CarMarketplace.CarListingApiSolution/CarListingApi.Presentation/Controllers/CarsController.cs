using CarListingApi.Application.CQRS.Car.Command;
using CarListingApi.Application.CQRS.Car.Query;
using CarListingApi.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;

namespace CarListingApi.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CarsController : Controller
    {
        private readonly ISender _sender;
        private readonly ILogger<CarsController> _logger;
        public CarsController(ISender sender, ILogger<CarsController> logger)
        {
            _sender = sender ?? throw new ArgumentNullException(nameof(sender));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        //[Authorize(Roles = "Admin")]
        //[Authorize(Policy = "CheckAdminPolicy")]
        public async Task<ActionResult<IEnumerable<CarListingDto>>> GetCars(string? searchTerm = "", int pageNumber = 1, int pageSize = 10, string? sort = "price_asc")
        {
            var (carEntities, paginationMetadata) = await _sender.Send(new GetAllCarQuery(searchTerm, pageNumber, pageSize, sort));

            Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(paginationMetadata));
            return Ok(new SuccessResponse<IEnumerable<CarListingDto>>(carEntities, HttpContext.Request.Path));
        }

        [HttpGet("checkAuth")]
        //[Authorize(Roles = "Admin")]
        [Authorize(Policy = "CheckAdminPolicy")]
        public async Task<ActionResult<IEnumerable<CarListingDto>>> GetCarsCheckAuth(string? searchTerm = "", int pageNumber = 1, int pageSize = 10, string? sort = "price_asc")
        {
            var (carEntities, paginationMetadata) = await _sender.Send(new GetAllCarQuery(searchTerm, pageNumber, pageSize, sort));

            Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(paginationMetadata));
            return Ok(new SuccessResponse<IEnumerable<CarListingDto>>(carEntities, HttpContext.Request.Path));
        }

        [HttpGet("{carId}", Name = "GetCarById")]
        public async Task<IActionResult> GetCarByIdAsync(int carId)
        {
            _logger.LogInformation("Fetching car with ID {CarId}", carId);

            try
            {
                var car = await _sender.Send(new GetCarQuery(carId));
                if (car == null)
                {
                    _logger.LogWarning("Car with ID {CarId} not found", carId);
                    return NotFound(new BadRequestResponse(HttpContext.Request.Path, "Car not existed"));
                }

                _logger.LogInformation("Successfully fetched car with ID {CarId}", carId);
                return Ok(new SuccessResponse<CarListingDto>(car, HttpContext.Request.Path));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching car with ID {CarId}", carId);
                throw;
            }
        }

        [HttpGet("{carId}/withseller", Name = "GetCarWithSeller")]
        public async Task<IActionResult> GetCarWithSellerAsync(int carId)
        {
            var car = await _sender.Send(new GetCarWithSellerQuery(carId));
            if (car == null)
            {
                return NotFound(new BadRequestResponse(HttpContext.Request.Path, "Car not existed"));
            }

            return Ok(new SuccessResponse<CarWithSellerDto>(car, HttpContext.Request.Path));
        }

        [HttpPost]
        public async Task<IActionResult> CreateCarAsync(CarInputDto car)
        {
            //var assignerId = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var createdCarToReturn = await _sender.Send(new CreateCarCommand(car));
            return CreatedAtRoute("GetCarById",
                new
                {
                    carId = createdCarToReturn.Id,
                }
                , new SuccessResponse<CarListingDto>(createdCarToReturn, HttpContext.Request.Path, "Car created successfully"));
        }

        [HttpPost("restore/{carId}")]
        public async Task<IActionResult> RestoreCarAsync(int carId)
        {
            await _sender.Send(new RestoreCarCommand(carId));
            return NoContent();
        }

        [HttpPut("{carId}")]
        public async Task<IActionResult> UpdateCarAsync(int carId, CarInputDto updatedCar)
        {
            await _sender.Send(new UpdateCarCommand(carId, updatedCar));
            return NoContent();
        }

        [HttpDelete("soft/{carId}")]
        public async Task<ActionResult> DeleteCarAsync(int carId)
        {
            await _sender.Send(new SoftDeleteCarCommand(carId));
            return NoContent();
        }

        [HttpDelete("{carId}")]
        public async Task<ActionResult> DeleteCarWithoutSoftAsync(int carId)
        {
            await _sender.Send(new DeleteCarCommand(carId));
            return NoContent();
        }
    }
}
