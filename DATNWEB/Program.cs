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
using DATNWEB.Hubs;
using DATNWEB.Models;

QlPhimAnimeContext db = new QlPhimAnimeContext();
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
// thêm dịch vụ signalR
builder.Services.AddSignalR();
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
//signalr
var reviewhubs = db.Animes.Select(x => x.AnimeId).ToList();
app.UseEndpoints(endpoints =>
{
    foreach(var i in reviewhubs)
    {
        endpoints.MapHub<ReviewHub>("/reviewhub/"+i);
    }
    
});

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
