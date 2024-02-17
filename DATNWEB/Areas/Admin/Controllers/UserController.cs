using DATNWEB.Models;
using DATNWEB.Models.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace DATNWEB.Areas.Admin.Controllers
{
    [AuthForAdmin]
    [Area("admin")]
    public class UserController : Controller
    {
        QlPhimAnimeContext db = new QlPhimAnimeContext();
        [Route("admin/User")]
        [HttpGet]
        public IActionResult UserList(int? page)
        {
            int pagesize = 10;
            int pagenum = page == null || page < 0 ? 1 : page.Value;
            var a = db.Users.AsNoTracking().OrderBy(x => x.UserId);
            PagedList<User> lst = new PagedList<User>(a, pagenum, pagesize);
            return View(lst);
        }
        [Route("admin/Useradd")]
        [HttpGet]
        public IActionResult UsersAdd()
        {
            var UserWithMaxId = db.Users.OrderByDescending(x => x.UserId).FirstOrDefault();
            string id = "SP0001";
            if (UserWithMaxId != null)
            {
                AutoCode a = new AutoCode();
                id = a.GenerateMa(UserWithMaxId.UserId);
            }
            var direc = new User() { UserId = id };
            return View(direc);
        }
        [Route("Useradds")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UserAdds(User User)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(User);
                db.SaveChanges();
                return RedirectToAction("UserList");
            }
            return View(User);
        }
        [Route("admin/Useredit")]
        [HttpGet]
        public IActionResult UserEdit(string id)
        {
            var a = db.Users.Where(x => x.UserId == id).FirstOrDefault();
            return View(a);
        }
        [Route("Useredit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UserEdits(User User)
        {
            if (ModelState.IsValid)
            {
                db.Entry(User).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("UserList");
            }
            return View(User);
        }
        [Route("Userdelete")]
        [HttpGet]
        public IActionResult UserDelete(string id)
        {
            var Useranimes = db.UserSubscriptions.Where(x => x.UserId == id).ToList();
            if (Useranimes.Count() > 0)
            {
                TempData["DeleteError"] = "Can not delete the Service User!";
                return RedirectToAction("UserList");
            }
            db.Remove(db.Users.Find(id));
            db.SaveChanges();
            return RedirectToAction("UserList");
        }
    }
}
