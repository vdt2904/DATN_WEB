using DATNWEB.Models;
using DATNWEB.Models.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace DATNWEB.Areas.Admin.Controllers
{
    [AuthForAdmin]
    [Area("admin")]
    
    public class DirectorController : Controller
    {
        QlPhimAnimeContext db = new QlPhimAnimeContext();
        [Route("admin/director")]
        public IActionResult DirectorList(int? page)
        {
            int pagesize = 10;
            int pagenum = page == null || page < 0 ? 1 : page.Value;
            var a = db.Directors.AsNoTracking().OrderBy(x => x.DirectorId);
            PagedList<Director> lst = new PagedList<Director>(a, pagenum, pagesize);
            return View(lst);
        }
        [Route("admin/directoradd")]
        [HttpGet]
        public IActionResult DirectorsAdd () {
            var directorWithMaxId = db.Directors.OrderByDescending(x => x.DirectorId).FirstOrDefault();
            string id = "DI0001";
            if (directorWithMaxId != null)
            {
                AutoCode a = new AutoCode();
                id = a.GenerateMa(directorWithMaxId.DirectorId);
            }
            var direc = new Director() { DirectorId = id };
            return View(direc);
        }
        [Route("directoradds")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DirectorsAdd(Director director)
        {
            if (ModelState.IsValid)
            {
                db.Directors.Add(director);
                db.SaveChanges();
                return RedirectToAction("DirectorList");
            }
            return View(director);
        }
        [Route("admin/directoredit")]
        [HttpGet]
        public IActionResult DirectorEdit(string id)
        {
            var a = db.Directors.Where(x => x.DirectorId == id).FirstOrDefault();
            return View(a);
        }
        [Route("directoredit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DirectorEdit(Director director)
        {
            if (ModelState.IsValid)
            {
                db.Entry(director).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("DirectorList");
            }
            return View(director);
        }
        [Route("directordelete")]
        [HttpGet]
        public IActionResult DirectorDelete(string id)
        {
            var animes = db.Animes.Where(x=> x.DirectorId == id).ToList();
            if(animes.Count() >0)
            {
                TempData["DeleteError"] = "Can not delete the director!";
                return RedirectToAction("DirectorList");
            }
            db.Remove(db.Directors.Find(id));
            db.SaveChanges();
            return RedirectToAction("DirectorList");
        }
    }
}
