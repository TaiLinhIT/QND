// --- QND.Core/Models/MenuItem.cs ---
using Core.Models;
using System.ComponentModel.DataAnnotations;

namespace QND.Core.Models
{
    public class MenuItem
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; } // << Quan trọng cho tính toán
        public string Description { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public bool IsAvailable { get; set; } = true;

        // Navigation property (Nếu cần)
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}