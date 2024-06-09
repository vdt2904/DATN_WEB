using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using DATNWEB.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList;
using DATNWEB.Models.Authentication;

namespace DATNWEB.Areas.Admin.Controllers
{
    [AuthForAdmin]
    [Area("admin")]
    public class SeasonController : Controller
    {
        QlPhimAnimeContext db = new QlPhimAnimeContext();
        [Route("admin/Season")]
        [HttpGet]
        public IActionResult SeasonList(int? page)
        {
            int pagesize = 10;
            int pagenum = page == null || page < 0 ? 1 : page.Value;
            var a = db.Seasons.AsNoTracking().OrderBy(x => x.SeasonId);
            PagedList<Season> lst = new PagedList<Season>(a, pagenum, pagesize);
            return View(lst);
        }
        [Route("admin/Seasonadd")]
        [HttpGet]
        public IActionResult SeasonsAdd()
        {
            var SeasonWithMaxId = db.Seasons.OrderByDescending(x => x.SeasonId).FirstOrDefault();
            string id = "SE0001";
            if (SeasonWithMaxId != null)
            {
                AutoCode a = new AutoCode();
                id = a.GenerateMa(SeasonWithMaxId.SeasonId);
            }
            var direc = new Season() { SeasonId = id };
            return View(direc);
        }
        [Route("Seasonadds")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SeasonsAdd(Season Season, IFormFile img)
        {
            if (img == null)
            {
                ModelState.AddModelError("img", "Hãy thêm ảnh");
            }
            if (ModelState.IsValid && img != null)
            {
                using (var stream = img.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(img.FileName, stream),
                        PublicId = "DATN/Season/" + Season.SeasonId.ToString(),
                        Overwrite = true,
                    };
                    Account account = new Account(
                        "dbpbkj4pg",
                        "718584524965961",
                        "exe6MST-mc9nLkpubAWeazM7gLI");
                    Cloudinary cloudinary = new Cloudinary(account);
                    cloudinary.Api.Secure = true;
                    var uploadResult = cloudinary.Upload(uploadParams);
                    string cloudinaryUrl = uploadResult.SecureUrl.ToString();
                    Season.ImageUrl = cloudinaryUrl;
                }
                db.Seasons.Add(Season);
                db.SaveChanges();
                return RedirectToAction("SeasonList");
            }
            
            return View(Season);
        }
        [Route("admin/Seasonedit")]
        [HttpGet]
        public IActionResult SeasonEdit(string id)
        {
            var a = db.Seasons.Where(x => x.SeasonId == id).FirstOrDefault();
            return View(a);
        }
        [Route("Seasonedit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SeasonEdit(Season Season, IFormFile img)
        {
            ModelState.Remove("img");
            if (ModelState.IsValid)
            {
                if(img != null) {
                    using (var stream = img.OpenReadStream())
                    {
                        var uploadParams = new ImageUploadParams()
                        {
                            File = new FileDescription(img.FileName, stream),
                            PublicId = "DATN/Season/" + Season.SeasonId.ToString(),
                            Overwrite = true,
                        };
                        Account account = new Account(
                            "dbpbkj4pg",
                            "718584524965961",
                            "exe6MST-mc9nLkpubAWeazM7gLI");
                        Cloudinary cloudinary = new Cloudinary(account);
                        cloudinary.Api.Secure = true;
                        var uploadResult = cloudinary.Upload(uploadParams);
                    }
                }
                db.Entry(Season).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("SeasonList");
            }
            return View(Season);
        }
        [Route("Seasondelete")]
        [HttpGet]
        public IActionResult SeasonDelete(string id)
        {
             var Seasonanimes = db.Animes.Where(x => x.SeasonId == id).ToList();
            if (Seasonanimes.Count() > 0)
            {
                TempData["DeleteError"] = "Can not delete the Season!";
                return RedirectToAction("SeasonList");
            }
            db.Remove(db.Seasons.Find(id));
            db.SaveChanges();
            return RedirectToAction("SeasonList");
        }
    }
}
