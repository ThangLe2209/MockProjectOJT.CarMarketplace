using CarListingApi.Application.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarListingApi.Application.CQRS.Car.Validation
{
    public class CarInputValidator : AbstractValidator<CarInputDto>
    {
        public CarInputValidator()
        {
            RuleFor(car => car.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(200).WithMessage("Title must not exceed 200 characters.");

            RuleFor(car => car.Price)
                .GreaterThan(0).WithMessage("Price must be greater than 0.");
        }
    }
}
