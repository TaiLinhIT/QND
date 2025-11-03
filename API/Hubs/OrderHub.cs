// --- QND.API/Hubs/OrderHub.cs ---
using Microsoft.AspNetCore.SignalR;

namespace QND.API.Hubs // << Namespace phải khớp
{
    // OrderHub kế thừa từ Hub của SignalR
    public class OrderHub : Hub
    {
        // 1. Phương thức để Bếp tham gia nhóm (Kitchen Group)
        public async Task JoinKitchenGroup()
        {
            // Thêm ConnectionId của client (màn hình Bếp) vào nhóm 'Kitchen'
            await Groups.AddToGroupAsync(Context.ConnectionId, "Kitchen");
        }

        // 2. Phương thức để Khách hàng tham gia nhóm bàn của họ
        public async Task JoinTableGroup(string tableCode)
        {
            // Thêm ConnectionId của client (Khách hàng) vào nhóm theo mã bàn (ví dụ: 'A05')
            await Groups.AddToGroupAsync(Context.ConnectionId, tableCode);
        }

        // *Lưu ý: Các phương thức gửi thông báo (SendAsync) sẽ được gọi từ OrderController
        // thông qua IHubContext<OrderHub>, chứ không gọi trực tiếp ở đây.*
    }
}