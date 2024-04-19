﻿using DATNWEB.Models;
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
            var a = db.Episodes.Where(x => x.AnimeId == id && x.Ep == ep).FirstOrDefault();
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
        public IActionResult pay()
        {
            return View();
        }
        public IActionResult package()
        {
            return View();
        }
    }
}