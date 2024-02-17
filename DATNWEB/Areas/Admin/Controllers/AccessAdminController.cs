using DATNWEB.Models;
using Microsoft.AspNetCore.Mvc;

namespace DATNWEB.Areas.Admin.Controllers
{
    [Area("admin")]
    public class AccessAdminController : Controller
    {
        QlPhimAnimeContext db = new QlPhimAnimeContext();
        [Route("admin/login")]
        [HttpGet]
        public IActionResult LoginAdmin()
        {
            if (HttpContext.Session.GetString("UserName") == null)
            {
                return View();
            }
            return RedirectToAction("Index","Admin");
        }
        [Route("admin/login")]
        [HttpPost]
        public IActionResult LoginAdmin(DATNWEB.Models.Admin admin)
        {
            AutoCode at = new AutoCode();
            var a = db.Admins.Where(x=>x.UserName== admin.UserName && x.PassWord == at.HashPassword(admin.PassWord)).FirstOrDefault();
            if (a != null)
            {
                HttpContext.Session.SetString("UserName", a.UserName);
                return RedirectToAction("Index", "Admin");
            }
            return View();
        }
        [Route("admin/Logout")]
        [HttpGet]
        public IActionResult LogoutAdmin()
        {
            HttpContext.Session.Remove("UserName");
            return RedirectToAction("LoginAdmin");
        }
    }
}
