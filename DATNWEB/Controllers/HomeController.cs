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
            var a = db.Animes.Find(id);
            return View(a);
        }
        public IActionResult Watch(string id)
        {
            var a = db.Animes.Find(id);
            return View(a);
        }
        public IActionResult Episode(string id, int ep)
        {
            var a = db.Episodes.Where(x => x.AnimeId == id && x.Ep == ep).FirstOrDefault();
            return View(a);
        }
        public IActionResult Login()
        {
            if(HttpContext.Session.GetString("UID") == null)
            {
                return View();
            }
            return RedirectToAction("Index", "Home");
        }
        
    }
}