using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderApi.Domain.Entities
{
    public class Order : BaseEntity
    {
        public int Id { get; set; }
        public int BuyerId { get; set; }
        // public DateTime OrderedDate { get; set; }
        public decimal TotalPrice { get; set; }
        public bool IsDeleted { get; set; } = false;
        public string Status { get; set; } = "Pending";

        // Navigation property
        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
    }
}
