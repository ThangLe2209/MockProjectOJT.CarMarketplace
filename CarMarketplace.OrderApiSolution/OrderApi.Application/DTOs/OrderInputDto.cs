using OrderApi.Domain.Entities;

namespace OrderApi.Application.DTOs
{
    public class OrderInputDto
    {
        public int BuyerId { get; set; }
        public string Status { get; set; } = "Pending";
        public List<OrderItemInputDto> Items { get; set; } = new();
    }

    public class OrderItemInputDto
    {
        public int CarId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
