using DATNWEB.Models;
using DATNWEB.Models.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace DATNWEB.Areas.Admin.Controllers
{
    [AuthForAdmin]
    [Area("admin")]
    public class GenreController : Controller
    {
        QlPhimAnimeContext db = new QlPhimAnimeContext();
        [Route("admin/genre")]
        [HttpGet]
        public IActionResult GenreList(int? page)
        {
            int pagesize = 10;
            int pagenum = page == null || page < 0 ? 1 : page.Value;
            var a = db.Genres.AsNoTracking().OrderBy(x => x.GenreId);
            PagedList<Genre> lst = new PagedList<Genre>(a, pagenum, pagesize);
            return View(lst);
        }
        [Route("admin/genreadd")]
        [HttpGet]
        public IActionResult genresAdd()
        {
            var genreWithMaxId = db.Genres.OrderByDescending(x => x.GenreId).FirstOrDefault();
            string id = "GE0001";
            if (genreWithMaxId != null)
            {
                AutoCode a = new AutoCode();
                id = a.GenerateMa(genreWithMaxId.GenreId);
            }
            var direc = new Genre() { GenreId = id };
            return View(direc);
        }
        [Route("genreadds")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult genreAdds(Genre genre)
        {
            if (ModelState.IsValid)
            {
                db.Genres.Add(genre);
                db.SaveChanges();
                return RedirectToAction("genreList");
            }
            return View(genre);
        }
        [Route("admin/genreedit")]
        [HttpGet]
        public IActionResult GenreEdit(string id)
        {
            var a = db.Genres.Where(x => x.GenreId == id).FirstOrDefault();
            return View(a);
        }
        [Route("genreedit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult GenreEdits(Genre genre)
        {
            if (ModelState.IsValid)
            {
                db.Entry(genre).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("genreList");
            }
            return View(genre);
        }
        [Route("genredelete")]
        [HttpGet]
        public IActionResult GenreDelete(string id)
        {
            var genreanimes = db.FilmGenres.Where(x => x.GenreId == id).ToList();
            if (genreanimes.Count() > 0)
            {
                db.FilmGenres.RemoveRange(genreanimes);
                db.SaveChanges();
            }
            db.Remove(db.Genres.Find(id));
            db.SaveChanges();
            return RedirectToAction("genreList");
        }
    }
}
