using CarListingApi.Application.CQRS.Car.Command;
using CarListingApi.Application.CQRS.Car.Query;
using CarListingApi.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CarListingApi.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CarsController : Controller
    {
        private readonly ISender _sender;
        public CarsController(ISender sender)
        {
            _sender = sender ?? throw new ArgumentNullException(nameof(sender));
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<CarListingDto>>> GetCars()
        {
            var results = await _sender.Send(new GetAllCarQuery());
            return Ok(new SuccessResponse<IEnumerable<CarListingDto>>(results, HttpContext.Request.Path));
        }

        [HttpGet("{carId}", Name = "GetCarById")]
        public async Task<IActionResult> GetCarByIdAsync(int carId)
        {
            var car = await _sender.Send(new GetCarQuery(carId));
            if (car == null)
            {
                return NotFound(new BadRequestResponse(HttpContext.Request.Path, "Car not existed"));
            }

            return Ok(new SuccessResponse<CarListingDto>(car, HttpContext.Request.Path));
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

        [HttpPut("{carId}")]
        public async Task<IActionResult> UpdateCarAsync(int carId, CarInputDto updatedCar)
        {
            await _sender.Send(new UpdateCarCommand(carId, updatedCar));
            return NoContent();
        }

        [HttpDelete("{carId}")]
        public async Task<ActionResult> DeleteCarAsync(int carId)
        {
            await _sender.Send(new DeleteCarCommand(carId));
            return NoContent();
        }
    }
}
