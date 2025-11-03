// --- QND.Client/Program.cs ---
using Client;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;


var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Thêm dịch vụ HTTPClient để gọi API
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// ĐĂNG KÝ DỊCH VỤ SIGNALRService
builder.Services.AddSingleton<SignalRService>(); // << Dòng code quan trọng

await builder.Build().RunAsync();