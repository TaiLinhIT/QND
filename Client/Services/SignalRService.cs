// --- QND.Client/Services/SignalRService.cs (Ví dụ) ---
using Microsoft.AspNetCore.SignalR.Client;

public class SignalRService
{
    private HubConnection _hubConnection;

    public async Task StartConnection(string tableCode)
    {
        _hubConnection = new HubConnectionBuilder()
            .WithUrl("https://localhost:7001/orderHub") // Thay bằng địa chỉ API của bạn
            .Build();

        // Xử lý sự kiện nhận thông báo cập nhật trạng thái
        _hubConnection.On<int, string>("OrderStatusUpdated", (orderId, status) =>
        {
            Console.WriteLine($"Order {orderId} updated to: {status}");
            // Cần có logic để cập nhật UI của khách hàng ở đây
        });

        await _hubConnection.StartAsync();

        // Khách hàng tham gia nhóm bàn của họ
        await _hubConnection.SendAsync("JoinTableGroup", tableCode);
    }

    // ... Thêm logic kết nối lại, ngắt kết nối
}