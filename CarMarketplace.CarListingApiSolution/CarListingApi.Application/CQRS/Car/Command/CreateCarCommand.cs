using CarListingApi.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarListingApi.Application.CQRS.Car.Command
{
    public record CreateCarCommand(CarInputDto CarInput) : IRequest<CarListingDto>;
}
