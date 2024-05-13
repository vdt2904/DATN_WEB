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
using DATNWEB.helpter;
using DATNWEB.Service;
using DATNWEB.Payments.Momo.Config;
using Net.payOS;
using Hangfire;
using Hangfire.SqlServer;
using DATNWEB.HangFire;

QlPhimAnimeContext db = new QlPhimAnimeContext();
var builder = WebApplication.CreateBuilder(args);
// Thiết lập cấu hình
builder.Configuration.AddJsonFile("appsettings.json");
var configuration = builder.Configuration;
//pay os
PayOS payOS = new PayOS(configuration["Environment:PAYOS_CLIENT_ID"] ?? throw new Exception("Cannot find environment"),
                    configuration["Environment:PAYOS_API_KEY"] ?? throw new Exception("Cannot find environment"),
                    configuration["Environment:PAYOS_CHECKSUM_KEY"] ?? throw new Exception("Cannot find environment"));
builder.Services.AddSingleton(payOS);
builder.Services.AddControllers();
// Add services to the container.
builder.Services.AddControllersWithViews();
// cấu hình mail
builder.Services.Configure<MailSetting>(builder.Configuration.GetSection("MailSetting"));
builder.Services.AddTransient<IMailService, MailService>();
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
//hangfire config
builder.Services.AddHangfire(configurations => configurations
        .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
        .UseSimpleAssemblyNameTypeSerializer()
        .UseRecommendedSerializerSettings()
        .UseSqlServerStorage(configuration.GetConnectionString("HangfireConnection")));
builder.Services.AddHangfireServer();
// thêm dịch vụ signalR
builder.Services.AddSignalR();
// login gg fb
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
//PayMent config
builder.Services.Configure<MomoConfig>(
    builder.Configuration.GetSection(MomoConfig.ConfigName));
//cors
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddMvc();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.WithOrigins("*").AllowAnyHeader().AllowAnyMethod();
        });
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
//hangfire
app.UseHangfireDashboard();
RecurringJob.AddOrUpdate<UpdateScheduledTasks>("RunDailyTask", x => x.RunDailyTask(), Cron.Daily(12, 00)); // Lập lịch chạy vào 12h hàng ngày
//RecurringJob.AddOrUpdate<UpdateScheduledTasks>("RunDailyTask", x => x.RunDailyTask(), Cron.MinuteInterval(1));

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
var commenthubs = db.Episodes.Select(x => x.EpisodeId).ToList();
app.UseEndpoints(endpoints =>
{
    foreach(var i in reviewhubs)
    {
        endpoints.MapHub<ReviewHub>("/reviewhub/"+i);
    }
    foreach(var i in commenthubs)
    {
        endpoints.MapHub<ReviewHub>("/commenthub/" + i);
    }
    // Định nghĩa route cho trang được gọi từ payOS
   /* endpoints.MapControllerRoute(
        name: "payment",
        pattern: "home/infouser",
        defaults: new { controller = "infouser", action = "ProcessPayment" });*/

});

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
