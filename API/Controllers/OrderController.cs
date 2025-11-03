// --- QND.API/Controllers/OrderController.cs ---
using Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using QND.API.Data;
using QND.API.Hubs;
// Cần tạo DTOs (Data Transfer Objects) cho dữ liệu gửi/nhận
// Tạm thời dùng Models trực tiếp cho đơn giản

[Route("api/[controller]")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IHubContext<OrderHub> _hubContext;

    public OrderController(ApplicationDbContext context, IHubContext<OrderHub> hubContext)
    {
        _context = context;
        _hubContext = hubContext;
    }

    // POST: api/Order/place
    // Nhận đơn hàng từ Khách hàng
    [HttpPost("place")]
    public async Task<ActionResult<Order>> PlaceOrder([FromBody] Order newOrder)
    {
        // 1. Logic nghiệp vụ (Kiểm tra TableId, tính TotalAmount)
        var table = await _context.Tables.FindAsync(newOrder.TableId);
        if (table == null) return NotFound("Table not found.");

        table.IsOccupied = true; // Cập nhật bàn đã có người
        newOrder.Status = "Pending";
        newOrder.OrderTime = DateTime.UtcNow;

        _context.Orders.Add(newOrder);

        // Đơn giản hóa: Cần tính TotalAmount thực tế ở đây
        // ... 

        await _context.SaveChangesAsync();

        // 2. Gửi thông báo real-time đến Bếp
        await _hubContext.Clients.Group("Kitchen")
            .SendAsync("ReceiveNewOrder", newOrder.Id, table.TableCode);

        return CreatedAtAction(nameof(GetOrder), new { id = newOrder.Id }, newOrder);
    }

    // GET: api/Order/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Order>> GetOrder(int id)
    {
        var order = await _context.Orders
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order == null)
        {
            return NotFound();
        }

        return order;
    }

    // PUT: api/Order/update-status/5?status=Preparing
    // Cập nhật trạng thái (Bếp gọi)
    [HttpPut("update-status/{id}")]
    public async Task<IActionResult> UpdateOrderStatus(int id, [FromQuery] string status)
    {
        var order = await _context.Orders.FindAsync(id);
        if (order == null) return NotFound();

        order.Status = status;
        await _context.SaveChangesAsync();

        // 3. Gửi thông báo đến Khách hàng (bàn đó)
        var table = await _context.Tables.FindAsync(order.TableId);
        if (table != null)
        {
            await _hubContext.Clients.Group(table.TableCode)
                .SendAsync("OrderStatusUpdated", id, status);
        }

        return NoContent();
    }
}