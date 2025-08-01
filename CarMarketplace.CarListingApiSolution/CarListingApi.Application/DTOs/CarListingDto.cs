using CarListingApi.Domain.Entities;

namespace CarListingApi.Application.DTOs
{
    public class CarListingDto : BaseEntity
    {
        public int Id { get; set; }
        public string Title { get; set; } = default!;
        public string Description { get; set; } = default!;
        public decimal Price { get; set; }
        public string Make { get; set; } = default!;
        public string Model { get; set; } = default!;
        public int Year { get; set; }
        public int Mileage { get; set; }
        public string Color { get; set; } = default!;
        public int SellerId { get; set; }
    }
}
