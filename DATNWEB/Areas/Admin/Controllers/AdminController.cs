using DATNWEB.Models;
using DATNWEB.Models.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace DATNWEB.Areas.Admin.Controllers
{
    [AuthForAdmin]
    [Area("admin")]
    public class AdminController : Controller
    {
        QlPhimAnimeContext db = new QlPhimAnimeContext();
        [Route("admin")]
        public IActionResult Index()
        {
            return View();
        }
    }
}

