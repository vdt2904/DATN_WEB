using DATNWEB.Models;
using DATNWEB.Models.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace DATNWEB.Areas.Admin.Controllers
{
    [AuthForAdmin]
    [Area("admin")]
    public class FilmGenreController : Controller
    {
        QlPhimAnimeContext db = new QlPhimAnimeContext();
        [Route("admin/FilmGenre")]
        public IActionResult FilmGenreList(int? page)
        {
            int pagesize = 10;
            int pagenum = page == null || page < 0 ? 1 : page.Value;
            var a = db.FilmGenres.AsNoTracking().OrderBy(x => x.Id);
            PagedList<FilmGenre> lst = new PagedList<FilmGenre>(a, pagenum, pagesize);
            return View(lst);
        }
        [Route("admin/FilmGenreadd")]
        [HttpGet]
        public IActionResult FilmGenresAdd()
        {
            ViewBag.Anime = new SelectList(db.Animes.ToList(), "AnimeId", "AnimeName");
            ViewBag.Genre = new SelectList(db.Genres.ToList(), "GenreId", "GenreName");
            return View();
        }
        [Route("FilmGenreadds")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult FilmGenresAdds(FilmGenre FilmGenre)
        {
            if (ModelState.IsValid)
            {
                var a = db.FilmGenres.AsNoTracking().Where(x => x.AnimeId == FilmGenre.AnimeId).Where(x=>x.GenreId == FilmGenre.GenreId).ToList();
                if (a.Count >0)
                {
                    TempData["AddError"] = "Can not add the Film genre!";
                    return RedirectToAction("FilmGenresAdd");
                }
                db.FilmGenres.Add(FilmGenre);
                db.SaveChanges();
                return RedirectToAction("FilmGenreList");
            }
            return View(FilmGenre);
        }
        [Route("admin/FilmGenreedit")]
        [HttpGet]
        public IActionResult FilmGenreEdit(int id)
        {
            var a = db.FilmGenres.Where(x => x.Id == id).FirstOrDefault();
            ViewBag.Anime = new SelectList(db.Animes.ToList(), "AnimeId", "AnimeName");
            ViewBag.Genre = new SelectList(db.Genres.ToList(), "GenreId", "GenreName");
            return View(a);
        }
        [Route("FilmGenreedit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult FilmGenreEdits(FilmGenre FilmGenre)
        {
            if (ModelState.IsValid)
            {
                var a = db.FilmGenres.AsNoTracking().Where(x => x.AnimeId == FilmGenre.AnimeId).Where(x => x.GenreId == FilmGenre.GenreId).Where(x=>x.Id != FilmGenre.Id).ToList();
                if (a.Count > 0)
                {
                    TempData["AddError"] = "Can not edit the Film genre!";
                    return RedirectToAction("FilmGenreEdit" , new {id = FilmGenre.Id});
                }
                db.Entry(FilmGenre).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("FilmGenreList");
            }
            return View(FilmGenre);
        }
        [Route("FilmGenredelete")]
        [HttpGet]
        public IActionResult FilmGenreDelete(int id)
        {
            db.Remove(db.FilmGenres.Find(id));
            db.SaveChanges();
            return RedirectToAction("FilmGenreList");
        }
    }
}
