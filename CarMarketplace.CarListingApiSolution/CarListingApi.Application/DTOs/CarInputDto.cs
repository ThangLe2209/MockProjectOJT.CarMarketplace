using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarListingApi.Application.DTOs
{
    public class CarInputDto
    {
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
        public string Status { get; set; } = default!;
        public int Quantity { get; set; }
    }
}
