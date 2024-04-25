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
    public class PackageUsageController : Controller
    {
        QlPhimAnimeContext db = new QlPhimAnimeContext();
        [Route("admin/serviceusage")]
        public IActionResult serviceList(string ids,int? page)
        {
            ViewBag.pid = ids;
            int pagesize = 10;
            int pagenum = page == null || page < 0 ? 1 : page.Value;
            var a = db.ServiceUsages.Where(x=> x.PackageId == ids).AsNoTracking().OrderBy(x => x.Price);
            PagedList<ServiceUsage> lst = new PagedList<ServiceUsage>(a, pagenum, pagesize);
            return View(lst);
        }
        [Route("admin/serviceadd")]
        [HttpGet]
        public IActionResult ServicesAdd(string id)
        {
            var direc = new ServiceUsage() { PackageId = id };
            return View(direc);
        }
        [Route("serviceadds")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ServiceAdds(ServiceUsage Package)
        {
            if (ModelState.IsValid)
            {
                db.ServiceUsages.Add(Package);
                db.SaveChanges();
                return Redirect("/admin/serviceusage?ids=" + Package.PackageId);

            }
            return View(Package);
        }
        [Route("admin/seviceedit")]
        [HttpGet]
        public IActionResult seviceedit(int id)
        {
            var a = db.ServiceUsages.Where(x => x.Id == id).FirstOrDefault();
            return View(a);
        }
        [Route("seviceedit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult seviceedits(ServiceUsage Package)
        {
            if (ModelState.IsValid)
            {
                db.Entry(Package).State = EntityState.Modified;
                db.SaveChanges();
                return Redirect("/admin/serviceusage?ids=" + Package.PackageId);

            }
            return View(Package);
        }
        [Route("Sevicedelete")]
        [HttpGet]
        public IActionResult ServiceDelete(int id)
        {
            var a = db.ServiceUsages.Find(id);
            db.Remove(db.ServiceUsages.Find(id));
            db.SaveChanges();
            return Redirect("/admin/serviceusage?ids=" + a.PackageId);
        }
    }
}
