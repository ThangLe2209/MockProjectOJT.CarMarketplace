using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarListingApi.Domain.Entities
{
    public class CarListing : BaseEntity
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
        public string Image { get; set; } = default!;
        public int SellerId { get; set; }
        public bool IsDeleted { get; set; } = false;
        public string Status { get; set; } = "Available"; // e.g., Available, Reserved, Sold
        public int Quantity { get; set; } = 1; // Default to 1, update as needed
    }
}
