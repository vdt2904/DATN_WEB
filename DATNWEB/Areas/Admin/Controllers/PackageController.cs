using DATNWEB.Models;
using DATNWEB.Models.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace DATNWEB.Areas.Admin.Controllers
{
    [AuthForAdmin]
    [Area("admin")]
    public class PackageController : Controller
    {
        QlPhimAnimeContext db = new QlPhimAnimeContext();
        [Route("admin/package")]
        [HttpGet]
        public IActionResult PackageList(int? page)
        {
            int pagesize = 10;
            int pagenum = page == null || page < 0 ? 1 : page.Value;
            var a = db.ServicePackages.AsNoTracking().OrderBy(x => x.PackageId);
            PagedList<ServicePackage> lst = new PagedList<ServicePackage>(a, pagenum, pagesize);
            return View(lst);
        }
        [Route("admin/Packageadd")]
        [HttpGet]
        public IActionResult PackagesAdd()
        {
            var PackageWithMaxId = db.ServicePackages.OrderByDescending(x => x.PackageId).FirstOrDefault();
            string id = "SP0001";
            if (PackageWithMaxId != null)
            {
                AutoCode a = new AutoCode();
                id = a.GenerateMa(PackageWithMaxId.PackageId);
            }
            var direc = new ServicePackage() { PackageId = id };
            return View(direc);
        }
        [Route("Packageadds")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult PackageAdds(ServicePackage Package)
        {
            if (ModelState.IsValid)
            {
                db.ServicePackages.Add(Package);
                db.SaveChanges();
                return RedirectToAction("PackageList");
            }
            return View(Package);
        }
        [Route("admin/Packageedit")]
        [HttpGet]
        public IActionResult PackageEdit(string id)
        {
            var a = db.ServicePackages.Where(x => x.PackageId == id).FirstOrDefault();
            return View(a);
        }
        [Route("Packageedit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult PackageEdits(ServicePackage Package)
        {
            if (ModelState.IsValid)
            {
                db.Entry(Package).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("PackageList");
            }
            return View(Package);
        }
        [Route("Packagedelete")]
        [HttpGet]
        public IActionResult PackageDelete(string id)
        {
            var Packageanimes = db.UserSubscriptions.Where(x => x.PackageId == id).ToList();
            if (Packageanimes.Count() > 0)
            {
                TempData["DeleteError"] = "Can not delete the Service Package!";
                return RedirectToAction("PackageList");
            }
            db.Remove(db.ServicePackages.Find(id));
            db.SaveChanges();
            return RedirectToAction("PackageList");
        }
    }
}

