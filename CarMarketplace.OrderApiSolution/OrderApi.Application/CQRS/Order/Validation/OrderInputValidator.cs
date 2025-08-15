using FluentValidation;
using OrderApi.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace OrderApi.Application.CQRS.Car.Validation
{
    public class OrderInputValidator : AbstractValidator<OrderInputDto>
    {
        public OrderInputValidator()
        {
            RuleFor(order => order.BuyerId)
               .GreaterThan(0).WithMessage("Must have buyer.");

            RuleFor(order => order.Status)
                .NotEmpty().WithMessage("Status is required.")
                .MaximumLength(200).WithMessage("Status must not exceed 200 characters.");

            RuleFor(order => order.Items)
                .NotEmpty().WithMessage("Order must have at least one item.");

            RuleForEach(order => order.Items).ChildRules(items =>
            {
                items.RuleFor(i => i.CarId)
                    .GreaterThan(0).WithMessage("CarId must be greater than 0.");

                items.RuleFor(i => i.Quantity)
                    .GreaterThan(0).WithMessage("Quantity must be greater than 0.");

                items.RuleFor(i => i.Price)
                    .GreaterThan(0).WithMessage("Price must be greater than 0.");
            });
        }
    }
}
