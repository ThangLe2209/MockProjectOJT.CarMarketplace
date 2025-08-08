using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationApi.Domain.Entities
{
    public class OutboxEvent : BaseEntity  // Add this inheritance
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(250)]
        public string Type { get; set; } = string.Empty; // e.g., "UserRegisteredEvent"

        [Required]
        public string Payload { get; set; } = string.Empty; // JSON serialized event

        public bool IsPublished { get; set; } = false;

        public string? Headers { get; set; }

        // Remove CreatedAt since it's inherited from BaseEntity as CreatedDate
    }
}
