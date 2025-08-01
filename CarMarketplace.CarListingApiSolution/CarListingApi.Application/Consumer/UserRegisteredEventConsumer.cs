using CarMarketplace.Contracts.Events;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarListingApi.Application.Consumer
{
    public class UserRegisteredEventConsumer : IConsumer<UserRegisteredEvent>
    {
        public Task Consume(ConsumeContext<UserRegisteredEvent> context)
        {
            var userId = context.Message.UserId;
            var email = context.Message.Email;
            // Demo: Log or handle the event
            Console.WriteLine($"Received UserRegisteredEvent: {userId}, {email}");
            return Task.CompletedTask;
        }
    }
}
