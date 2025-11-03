using System.ComponentModel.DataAnnotations;

namespace Core.Models
{
    public class Table
    {
        [Key]
        public int Id { get; set; }
        public string TableCode { get; set; } = string.Empty;
        public string QrCodeUrl { get; set; } = string.Empty;
        public bool IsOccupied { get; set; } = false;

        // Navigation property
        public ICollection<Order> Orders { get; set; } = new List<Order>();

    }
}
