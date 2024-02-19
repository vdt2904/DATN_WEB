using DATNWEB.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DATNWEB.Controllers
{
    public class HomeController : Controller
    {
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
    }
}