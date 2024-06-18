using DATNWEB.Models;
using Microsoft.AspNetCore.Mvc;
using Net.payOS.Types;
using Net.payOS;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace DATNWEB.Controllers
{
    public class HomeController : Controller
    {
        private readonly PayOS _payOS;
        private readonly IConfiguration _configuration;
        private readonly IDistributedCache _distributedCache;
        public HomeController(PayOS payos, IConfiguration configuration, IDistributedCache distributedCache)
        {
            _payOS = payos;
            _configuration = configuration;
            _distributedCache = distributedCache;
        }
        QlPhimAnimeContext db = new QlPhimAnimeContext();
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult category(string Genre)
        {
            var genre = db.Genres.Find(Genre);
            return View(genre);
        }
        public IActionResult trending()
        {
            return View();
        }
        public IActionResult popular()
        {
            return View();
        }
        public IActionResult recently()
        {
            return View();
        }
        public IActionResult Blog()
        {
            return View();
        }
        public IActionResult Blogdetail(string season)
        {
            var a = db.Seasons.Find(season);
            return View(a);
        }
        public IActionResult AnimeDetail(string id)
        {
            //session
            var sessionInfoJson = HttpContext.Session.GetString("SessionInfo");
            if (string.IsNullOrEmpty(sessionInfoJson))
            {
                return RedirectToAction("Login", "home");
            }
            var sessionInfo = JsonConvert.DeserializeObject<dynamic>(sessionInfoJson);
            string userId = sessionInfo.UID;
            string clientId = sessionInfo.ClientId;
            //redis
            string storedinfoJson = _distributedCache.GetString(userId);
            if (string.IsNullOrEmpty(storedinfoJson))
            {
                return RedirectToAction("Login", "home");
            }
            var clientInfo = JsonConvert.DeserializeObject<dynamic>(storedinfoJson);
            string storedUserId = clientInfo.UID;
            string storedClientId = clientInfo.ClientId;
            if (clientId != storedClientId || userId != storedUserId)
            {
                return RedirectToAction("Login", "home");
            }
            var a = db.Animes.Find(id);
            return View(a);
        }
        public IActionResult Episode(string id, int ep)
        {
            //session
            var sessionInfoJson = HttpContext.Session.GetString("SessionInfo");
            if (string.IsNullOrEmpty(sessionInfoJson))
            {
                return RedirectToAction("login", "Home");
            }
            var sessionInfo = JsonConvert.DeserializeObject<dynamic>(sessionInfoJson);
            string userId = sessionInfo.UID;
            string clientId = sessionInfo.ClientId;
            //redis
            string storedinfoJson = _distributedCache.GetString(userId);
            if (string.IsNullOrEmpty(storedinfoJson))
            {
                return RedirectToAction("login", "Home");
            }
            var clientInfo = JsonConvert.DeserializeObject<dynamic>(storedinfoJson);
            string storedUserId = clientInfo.UID;
            string storedClientId = clientInfo.ClientId;
            if (clientId != storedClientId || userId != storedUserId)
            {
                return RedirectToAction("login", "Home");
            }
            var ani = db.Animes.Find(id);
            var us = db.Users.Find(userId);
            var a = db.Episodes.Where(x => x.AnimeId == id && x.Ep == ep).FirstOrDefault();
            if (ani.Permission == 0 && us.UserType == 0 && ep != 0)
            {
                return RedirectToAction("package", "Home");
            }
            return View(a);
        }
        public IActionResult Login()
        {
            //session
            var sessionInfoJson = HttpContext.Session.GetString("SessionInfo");
            if (string.IsNullOrEmpty(sessionInfoJson))
            {
                return View();
            }
            var sessionInfo = JsonConvert.DeserializeObject<dynamic>(sessionInfoJson);
            string userId = sessionInfo.UID;
            string clientId = sessionInfo.ClientId;
            //redis
            string storedinfoJson = _distributedCache.GetString(userId);
            if (string.IsNullOrEmpty(storedinfoJson))
            {
                return View();
            }
            var clientInfo = JsonConvert.DeserializeObject<dynamic>(storedinfoJson);
            string storedUserId = clientInfo.UID;
            string storedClientId = clientInfo.ClientId;
            if (clientId != storedClientId || userId != storedUserId)
            {
                return View();
            }
            return RedirectToAction("infouser", "Home");
        }
        public IActionResult Register()
        {
            return View();
        }
        public IActionResult infouser()
        {
            return View();
        }
        public IActionResult pay(string id)
        {
            var a = db.ServiceUsages.Where(x => x.PackageId == id).FirstOrDefault();
            return View(a);
        }
        public IActionResult package()
        {
            //session
            var sessionInfoJson = HttpContext.Session.GetString("SessionInfo");
            if (string.IsNullOrEmpty(sessionInfoJson))
            {
                return RedirectToAction("Login", "Home");
            }
            var sessionInfo = JsonConvert.DeserializeObject<dynamic>(sessionInfoJson);
            string userId = sessionInfo.UID;
            string clientId = sessionInfo.ClientId;
            //redis
            string storedinfoJson = _distributedCache.GetString(userId);
            if (string.IsNullOrEmpty(storedinfoJson))
            {
                return RedirectToAction("Login", "Home");
            }
            var clientInfo = JsonConvert.DeserializeObject<dynamic>(storedinfoJson);
            string storedUserId = clientInfo.UID;
            string storedClientId = clientInfo.ClientId;
            if (clientId != storedClientId || userId != storedUserId)
            {
                return RedirectToAction("Login", "Home");
            }
            return View();
        }
        public async Task<IActionResult> vietqr(int id)
        {
            var sessionInfoJson = HttpContext.Session.GetString("SessionInfo");
            var sessionInfo = JsonConvert.DeserializeObject<dynamic>(sessionInfoJson);
            string userId = sessionInfo.UID;
            try
            {
                // Sử dụng await để gọi đến cơ sở dữ liệu bất đồng bộ
                var service = await db.ServiceUsages.FindAsync(id);

                int orderCode = int.Parse(DateTimeOffset.Now.ToString("ffffff"));

                // Sử dụng await để gọi đến cơ sở dữ liệu bất đồng bộ
                var pack = await db.ServicePackages.FirstOrDefaultAsync(x => x.PackageId == service.PackageId);

                ItemData item = new ItemData(pack.PackageName, 1, service.Price);
                List<ItemData> items = new List<ItemData>();
                items.Add(item);

                PaymentData paymentData = new PaymentData(orderCode, service.Price, "#" + orderCode, items, _configuration["Url:baseUrl"] + "home/package", _configuration["Url:baseUrl"] + "home/infousers");
                CreatePaymentResult createPayment = await _payOS.createPaymentLink(paymentData);
                Bill b = new Bill
                {
                    Id = createPayment.paymentLinkId,
                    Userid = userId,
                    Description = createPayment.orderCode.ToString(),
                    Createat = DateTime.Now,
                    Ids = id
                };
                db.Bills.Add(b);
                db.SaveChanges();
                // Sử dụng await để gọi đến phương thức API bên ngoài bất đồng bộ

                return Redirect(createPayment.checkoutUrl);
            }
            catch (System.Exception exception)
            {
                Console.WriteLine(exception);
                return Redirect(_configuration["Url:baseUrl"]);
            }
        }
        public IActionResult payment(int a, int b)
        {
            switch (a)
            {
                case 1:
                    // Xử lý thanh toán bằng phương thức 1
                    return Redirect("/home/vietqr?id=" + b);

                case 2:
                    // Xử lý thanh toán bằng phương thức 2

                    return Redirect("/home/vietqr?id=" + b);

                case 3:
                    // Xử lý thanh toán bằng phương thức 3
                    return Redirect("/home/vietqr?id=" + b);

                default:
                    // Xử lý trường hợp không hợp lệ
                    return Redirect("/home/vietqr?id=" + b);
            }
        }
        public IActionResult history()
        {
            //session
            var sessionInfoJson = HttpContext.Session.GetString("SessionInfo");
            if (string.IsNullOrEmpty(sessionInfoJson))
            {
                return RedirectToAction("Login", "Home");
            }
            var sessionInfo = JsonConvert.DeserializeObject<dynamic>(sessionInfoJson);
            string userId = sessionInfo.UID;
            string clientId = sessionInfo.ClientId;
            //redis
            string storedinfoJson = _distributedCache.GetString(userId);
            if (string.IsNullOrEmpty(storedinfoJson))
            {
                return RedirectToAction("Login", "Home");
            }
            var clientInfo = JsonConvert.DeserializeObject<dynamic>(storedinfoJson);
            string storedUserId = clientInfo.UID;
            string storedClientId = clientInfo.ClientId;
            if (clientId != storedClientId || userId != storedUserId)
            {
                return RedirectToAction("Login", "Home");
            }
            return View();
        }
        public IActionResult Search(string keyword)
        {
            ViewBag.keyword = keyword;
            return View();
        }
    }
}