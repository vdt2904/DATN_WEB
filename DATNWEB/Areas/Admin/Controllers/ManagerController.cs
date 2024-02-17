using DATNWEB.Models;
using DATNWEB.Models.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace DATNWEB.Areas.Admin.Controllers
{
    [AuthForAdmin]
    [AuthForAccess]
    [Area("admin")]
    public class ManagerController : Controller
    {
        QlPhimAnimeContext db = new QlPhimAnimeContext();
        [Route("admin/Admin")]
        [HttpGet]
        public IActionResult AdminList(int? page)
        {
            int pagesize = 10;
            int pagenum = page == null || page < 0 ? 1 : page.Value;
            var a = db.Admins.AsNoTracking().OrderBy(x => x.Id);
            PagedList<DATNWEB.Models.Admin> lst = new PagedList<DATNWEB.Models.Admin>(a, pagenum, pagesize);
            return View(lst);
        }
        [Route("admin/Adminadd")]
        [HttpGet]
        public IActionResult AdminsAdd()
        {
            return View();
        }
        [Route("Adminadds")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AdminAdds(DATNWEB.Models.Admin Admin)
        {
            if (ModelState.IsValid)
            {
                AutoCode auto = new AutoCode();
                Admin.PassWord = auto.HashPassword(Admin.PassWord);
                db.Admins.Add(Admin);
                db.SaveChanges();
                return RedirectToAction("AdminList");
            }
            return View(Admin);
        }
        [Route("admin/Adminedit")]
        [HttpGet]
        public IActionResult AdminEdit(int id)
        {
            var a = db.Admins.Where(x => x.Id == id).FirstOrDefault();
            return View(a);
        }
        [Route("Adminedit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AdminEdits(DATNWEB.Models.Admin Admin)
        {
            if (ModelState.IsValid)
            {
                var a = db.Admins.AsNoTracking().Where(x => x.Id == Admin.Id).FirstOrDefault();
                if(a.PassWord != Admin.PassWord)
                {
                    AutoCode auto = new AutoCode();
                    Admin.PassWord = auto.HashPassword(Admin.PassWord);
                }
                db.Entry(Admin).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("AdminList");
            }
            return View(Admin);
        }
        [Route("Admindelete")]
        [HttpGet]
        public IActionResult AdminDelete(string id)
        {
            db.Remove(db.Admins.Find(id));
            db.SaveChanges();
            return RedirectToAction("AdminList");
        }
    }
}
