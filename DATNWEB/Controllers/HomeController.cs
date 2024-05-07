using DATNWEB.Models;
using Microsoft.AspNetCore.Mvc;
using Net.payOS.Types;
using Net.payOS;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace DATNWEB.Controllers
{
    public class HomeController : Controller
    {
        private readonly PayOS _payOS;
        public HomeController(PayOS payos)
        {
            _payOS = payos;
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
            if(HttpContext.Session.GetString("UID") == null)
            {
                return RedirectToAction("Login","home");
            }
            var a = db.Animes.Find(id);
            return View(a);
        }
        public IActionResult Watch(string id)
        {
            var a = db.Episodes.Where(x=>x.AnimeId == id && x.Ep == 0).FirstOrDefault();
            return Redirect($"/home/episode?id={a.AnimeId}&ep={a.Ep}");
        }
        public IActionResult Episode(string id, int ep)
        {
            var ani = db.Animes.Find(id);
            var us = db.Users.Find(HttpContext.Session.GetString("UID"));
            var a = db.Episodes.Where(x => x.AnimeId == id && x.Ep == ep).FirstOrDefault();
            if (ani.Permission == 0 && us.UserType ==0 && ep != 0)
            {
                return RedirectToAction("package", "Home");
            }
            return View(a);
        }
        public IActionResult Login()
        {
            if(HttpContext.Session.GetString("UID") == null)
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
            if(HttpContext.Session.GetString("UID") == null)
            {
                return RedirectToAction("Login", "Home");
            }
            return View();
        }
        
        public async Task<IActionResult> Checkout(int id)
        {
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

                PaymentData paymentData = new PaymentData(orderCode, service.Price, "Thanh toan #" + orderCode, items, "https://localhost:7274/home/package", "https://localhost:7274/home/infousers");
                CreatePaymentResult createPayment = await _payOS.createPaymentLink(paymentData);
                Bill b = new Bill
                {
                    Id = createPayment.paymentLinkId,
                    Userid = HttpContext.Session.GetString("UID"),
                    Description = paymentData.description,
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
                return Redirect("https://localhost:7274/");
            }
        }
    }
}