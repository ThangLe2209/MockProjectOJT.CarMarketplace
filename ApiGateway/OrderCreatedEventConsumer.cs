using CarMarketplace.Contracts.Events;
using MassTransit;
using Microsoft.AspNetCore.SignalR;

namespace ApiGateway
{
    public class OrderCreatedEventConsumer : IConsumer<OrderCreatedEvent>
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public OrderCreatedEventConsumer(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
        {
            foreach (var item in context.Message.Items)
            {
                await _hubContext.Clients.All.SendAsync("CarUpdated", item.CarId);
            }
        }
    }
}
