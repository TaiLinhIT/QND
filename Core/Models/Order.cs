namespace Core.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int TableId { get; set; }
        public Table Table { get; set; } // Khóa ngoại
        public DateTime OrderTime { get; set; } = DateTime.UtcNow;
        public string Status { get; set; } = "Pending"; // Pending, Preparing, Ready, Paid
        public decimal TotalAmount { get; set; }

        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
    }
}
