// --- QND.API/Program.cs ---
using Microsoft.EntityFrameworkCore;
using QND.API.Data;
using QND.API.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Cấu hình Database (ví dụ dùng In-Memory cho nhanh)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseInMemoryDatabase("QNDDatabase")); // Dùng UseSqlServer nếu cần

// Thêm SignalR
builder.Services.AddSignalR();

// Thêm Controllers và Swagger/OpenAPI
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Cấu hình CORS (quan trọng cho Blazor Client)
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
    {
        policy.AllowAnyHeader()
              .AllowAnyMethod()
              .WithOrigins("https://localhost:7002", "http://localhost:5110", // Client Ports (Client chạy ở 7002, HTTP 5110)
                           "https://localhost:7001", "http://localhost:5140")  // API Ports (API chạy ở 7001, HTTP 5140)
              .AllowCredentials();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("CorsPolicy"); // Sử dụng CORS

app.UseAuthorization();

// Map SignalR Hub
app.MapHub<OrderHub>("/orderHub");

app.MapControllers();

app.Run();