using CloudinaryDotNet;
using System.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Principal;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.CodeAnalysis.Options;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Net.WebSockets;
using System.Net;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.Facebook;

var builder = WebApplication.CreateBuilder(args);
// Thiết lập cấu hình
builder.Configuration.AddJsonFile("appsettings.json");
var configuration = builder.Configuration;
// Add services to the container.
builder.Services.AddControllersWithViews();
// Thêm đoạn mã sau vào phương thức ConfigureServices trong Startup.cs
builder.Services.Configure<KestrelServerOptions>(options =>
{
    options.Limits.MaxRequestBodySize = 104857600;
});
builder.Services.Configure<IISServerOptions>(options =>
{
    options.MaxRequestBodySize = 104857600;
});
builder.Services.AddDistributedMemoryCache(); // You can replace this with other distributed cache providers
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Set session timeout
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddAuthentication(options =>
                {
                    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
                })
                .AddCookie()
                .AddGoogle(GoogleDefaults.AuthenticationScheme, options =>
                {
                    options.ClientId = configuration.GetSection("Login:GoogleClientId").Value;
                    options.ClientSecret = configuration.GetSection("Login:GoogleClientSecret").Value;
                })
                .AddFacebook(FacebookDefaults.AuthenticationScheme,options =>
                {
                    options.AppId = configuration["Login:FacebookClientId"];
                    options.AppSecret = configuration["Login:FacebookClientSecret"];
                });

// cau hing jwt
var key = Encoding.UTF8.GetBytes(configuration["Jwt:Secret"]);
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidIssuers = new string[] { configuration["Jwt:Issuer"] },
        ValidAudiences = new string[] { configuration["Jwt:Issuer"] },
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication();
app.UseRouting();
var wsOptions = new WebSocketOptions { KeepAliveInterval = TimeSpan.FromSeconds(5) };
app.UseWebSockets(wsOptions); // Kích hoạt WebSocket support

/*app.Use(async (context, next) =>
{
    if (context.Request.Path == "/send-review")
    {
        if (context.WebSockets.IsWebSocketRequest)
        {
            using (WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync())
            {
                await Send(context, webSocket);
            }
        }
        else
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        }
    }
    else
    {
        await next();
    }
});

async Task Send(HttpContext context, WebSocket webSocket)
{
    var buffer = new byte[1024 * 4];
    WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), System.Threading.CancellationToken.None);
    if (result != null)
    {
        while (!result.CloseStatus.HasValue)
        {
            string msg = Encoding.UTF8.GetString(new ArraySegment<byte>(buffer, 0, result.Count));
            Console.WriteLine($"clients says: {msg}");
            await webSocket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes($"Server says: {DateTime.UtcNow:f}")), result.MessageType, result.EndOfMessage, System.Threading.CancellationToken.None);
            result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), System.Threading.CancellationToken.None);
            Console.WriteLine();
        }
    }
    await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, System.Threading.CancellationToken.None);
}*/

async Task HandleWebSocketConnection(WebSocket webSocket)
{
    try
    {
        var buffer = new byte[1024 * 4];
        while (webSocket.State == WebSocketState.Open)
        {
            var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            if (result.MessageType == WebSocketMessageType.Text)
            {
                // Nhận dữ liệu từ client
                var message = Encoding.UTF8.GetString(buffer, 0, result.Count);

                // Xử lý tin nhắn nếu cần
                // Ví dụ: Trong trường hợp này, không cần xử lý tin nhắn từ client

                // Gửi tin nhắn đến client để kích hoạt AJAX request
                var responseMessage = Encoding.UTF8.GetBytes("review ajax");
                await webSocket.SendAsync(new ArraySegment<byte>(responseMessage, 0, responseMessage.Length), result.MessageType, result.EndOfMessage, CancellationToken.None);
            }
        }
    }
    catch (WebSocketException ex)
    {
        // Xử lý các lỗi kết nối WebSocket nếu cần
        Console.WriteLine($"WebSocket connection error: {ex.Message}");
    }
}



app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
