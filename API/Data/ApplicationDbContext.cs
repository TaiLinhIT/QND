// --- QND.API/Data/ApplicationDbContext.cs ---

using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace QND.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Table> Tables { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        // ... DbSet<MenuItem>

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Khởi tạo dữ liệu mẫu cho bàn
            modelBuilder.Entity<Table>().HasData(
                new Table { Id = 1, TableCode = "A01", QrCodeUrl = "/menu/A01" },
                new Table { Id = 2, TableCode = "A02", QrCodeUrl = "/menu/A02" }
            );
        }
    }
}