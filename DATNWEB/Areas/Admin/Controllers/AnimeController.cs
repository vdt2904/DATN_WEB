using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using DATNWEB.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList;
using Microsoft.AspNetCore.Mvc.Rendering;
using DATNWEB.Models.Authentication;

namespace DATNWEB.Areas.Admin.Controllers
{
    [AuthForAdmin]
    [Area("admin")]
    public class AnimeController : Controller
    {
        QlPhimAnimeContext db = new QlPhimAnimeContext();
        [Route("admin/Anime")]
        [HttpGet]
        public IActionResult AnimeList(int? page)
        {
            int pagesize = 4;
            int pagenum = page == null || page < 0 ? 1 : page.Value;
            var a = db.Animes.AsNoTracking().OrderByDescending(x => x.AnimeId);
            PagedList<Anime> lst = new PagedList<Anime>(a, pagenum, pagesize);
            return View(lst);
        }
        [Route("admin/Animeadd")]
        [HttpGet]
        public IActionResult AnimesAdd()
        {
            var AnimeWithMaxId = db.Animes.OrderByDescending(x => x.AnimeId).FirstOrDefault();
            string id = "AN0001";
            if (AnimeWithMaxId != null)
            {
                AutoCode a = new AutoCode();
                id = a.GenerateMa(AnimeWithMaxId.AnimeId);
            }
            ViewBag.Season = new SelectList(db.Seasons.ToList(), "SeasonId", "SeasonName");
            ViewBag.Director = new SelectList(db.Directors.ToList(), "DirectorId", "DirectorName");
            var direc = new Anime() { AnimeId = id };
            return View(direc);
        }
        [Route("Animeadds")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AnimesAdd(Anime Anime, IFormFile h, IFormFile v, IFormFile b)
        {
            if (ModelState.IsValid)
            {
                Account account = new Account(
                    "dbpbkj4pg",
                    "718584524965961",
                    "exe6MST-mc9nLkpubAWeazM7gLI");
                Cloudinary cloudinary = new Cloudinary(account);
                cloudinary.Api.Secure = true;
                using (var hStream = h.OpenReadStream())
                {
                    var hUploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(h.FileName, hStream),
                        PublicId = "DATN/Anime/" + Anime.AnimeId.ToString() + "/h" + Anime.AnimeId.ToString(),
                        Overwrite = true,
                    };
                    var hUploadResult = cloudinary.Upload(hUploadParams);
                    Anime.ImageHUrl = hUploadResult.SecureUrl.ToString();
                }
                using (var vStream = v.OpenReadStream())
                {
                    var vUploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(v.FileName, vStream),
                        PublicId = "DATN/Anime/" + Anime.AnimeId.ToString() + "/v" + Anime.AnimeId.ToString(),
                        Overwrite = true,
                    };
                    var vUploadResult = cloudinary.Upload(vUploadParams);
                    Anime.ImageVUrl = vUploadResult.SecureUrl.ToString();
                }
                using (var bStream = b.OpenReadStream())
                {
                    var bUploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(b.FileName, bStream),
                        PublicId = "DATN/Anime/" + Anime.AnimeId.ToString() + "/b" + Anime.AnimeId.ToString(),
                        Overwrite = true,
                    };
                    var bUploadResult = cloudinary.Upload(bUploadParams);
                    Anime.BackgroundImageUrl = bUploadResult.SecureUrl.ToString();
                }
                db.Animes.Add(Anime);
                db.SaveChanges();
                return RedirectToAction("AnimeList");
            }
            return View(Anime);
        }
        [Route("admin/Animeedit")]
        [HttpGet]
        public IActionResult AnimeEdit(string id)
        {
            var a = db.Animes.Where(x => x.AnimeId == id).FirstOrDefault();
            ViewBag.Season = new SelectList(db.Seasons.ToList(), "SeasonId", "SeasonName");
            ViewBag.Director = new SelectList(db.Directors.ToList(), "DirectorId", "DirectorName");
            return View(a);
        }
        [Route("Animeedit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AnimeEdits(Anime Anime, IFormFile h, IFormFile v, IFormFile b)
        {
            ModelState.Remove("h");
            ModelState.Remove("v");
            ModelState.Remove("b");
            if (ModelState.IsValid)
            {
                if (h != null && v != null && b != null)
                {
                    Account account = new Account(
                    "dbpbkj4pg",
                    "718584524965961",
                    "exe6MST-mc9nLkpubAWeazM7gLI");
                    Cloudinary cloudinary = new Cloudinary(account);
                    cloudinary.Api.Secure = true;
                    using (var hStream = h.OpenReadStream())
                    {
                        var hUploadParams = new ImageUploadParams()
                        {
                            File = new FileDescription(h.FileName, hStream),
                            PublicId = "DATN/Anime/" + Anime.AnimeId.ToString() + "/h" + Anime.AnimeId.ToString(),
                            Overwrite = true,
                        };
                        var hUploadResult = cloudinary.Upload(hUploadParams);
                        Anime.ImageHUrl = hUploadResult.SecureUrl.ToString();
                    }
                    using (var vStream = v.OpenReadStream())
                    {
                        var vUploadParams = new ImageUploadParams()
                        {
                            File = new FileDescription(v.FileName, vStream),
                            PublicId = "DATN/Anime/" + Anime.AnimeId.ToString() + "/v" + Anime.AnimeId.ToString(),
                            Overwrite = true,
                        };
                        var vUploadResult = cloudinary.Upload(vUploadParams);
                        Anime.ImageVUrl = vUploadResult.SecureUrl.ToString();
                    }
                    using (var bStream = b.OpenReadStream())
                    {
                        var bUploadParams = new ImageUploadParams()
                        {
                            File = new FileDescription(b.FileName, bStream),
                            PublicId = "DATN/Anime/" + Anime.AnimeId.ToString() + "/b" + Anime.AnimeId.ToString(),
                            Overwrite = true,
                        };
                        var bUploadResult = cloudinary.Upload(bUploadParams);
                        Anime.BackgroundImageUrl = bUploadResult.SecureUrl.ToString();
                    }
                }
                db.Entry(Anime).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("AnimeList");
            }
            return RedirectToAction("AnimeEdit", new { id = Anime.AnimeId });
        }
        [Route("Animedelete")]
        [HttpGet]
        public IActionResult AnimeDelete(string id)
        {
            AutoCode at = new AutoCode();
            Account account = new Account(
                "dbpbkj4pg",
                "718584524965961",
                "exe6MST-mc9nLkpubAWeazM7gLI");
            Cloudinary cloudinary = new Cloudinary(account);
            cloudinary.Api.Secure = true;
            var Animeanimes = db.FilmGenres.Where(x => x.AnimeId == id).ToList();
            if (Animeanimes.Any())
            {
                db.RemoveRange(Animeanimes);
            }
            var reviews = db.Reviews.Where(x => x.AnimeId == id).ToList();
            if (reviews.Any())
            {
                db.RemoveRange(reviews);
            }
            var a = db.Animes.Where(x => x.AnimeId == id).FirstOrDefault();
            string imagePath = at.ExtractPathFromUrl(a.ImageHUrl);
            string imagePath1 = at.ExtractPathFromUrl(a.ImageVUrl);
            string imagePath2 = at.ExtractPathFromUrl(a.BackgroundImageUrl);
            var deletionParams = new DeletionParams(imagePath)
            {
                Invalidate = true,
                Type = "upload",
                ResourceType = ResourceType.Image
            };
            var deletionResult = cloudinary.Destroy(deletionParams);
            var deletionParams1 = new DeletionParams(imagePath1) 
            {
                Invalidate = true,
                Type = "upload",
                ResourceType = ResourceType.Image
            };
       
            var deletionResult1 = cloudinary.Destroy(deletionParams1);
            var deletionParams2 = new DeletionParams(imagePath2)
            {
                Invalidate = true,
                Type = "upload",
                ResourceType = ResourceType.Image
            }; ;
            var deletionResult2 = cloudinary.Destroy(deletionParams2);


            db.Remove(db.Animes.Find(id));
            db.SaveChanges();

            return RedirectToAction("AnimeList");
        }
    }
}
