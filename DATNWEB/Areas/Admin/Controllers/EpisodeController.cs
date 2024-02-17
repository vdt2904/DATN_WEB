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
    public class EpisodeController : Controller
    {
        private HttpClient httpClient = new HttpClient();
        QlPhimAnimeContext db = new QlPhimAnimeContext();
        [Route("admin/Episode")]
        [HttpGet]
        public IActionResult EpisodeList(string id ,int? page)
        {
            int pagesize = 10;
            int pagenum = page == null || page < 0 ? 1 : page.Value;
            var a = db.Episodes.AsNoTracking().OrderBy(x => x.EpisodeId).Where(x=>x.AnimeId == id);
            PagedList<Episode> lst = new PagedList<Episode>(a, pagenum, pagesize);
            ViewBag.AnimeId = id;
            return View(lst);
        }
        [Route("admin/Episodeadd")]
        [HttpGet]
        public IActionResult EpisodesAdd(string ani)
        {
            var EpisodeWithMaxId = db.Episodes.OrderByDescending(x => x.EpisodeId).FirstOrDefault();
            var epmax = db.Episodes.OrderByDescending(x => x.EpisodeId).Where(x => x.AnimeId == ani).FirstOrDefault();
            string id = "EP0001";
            int? ep = 0;
            if (EpisodeWithMaxId != null)
            {
                AutoCode a = new AutoCode();
                id = a.GenerateMa(EpisodeWithMaxId.EpisodeId);
                
            }
            if(epmax != null)
            {
                ep = epmax.Ep + 1;
            }
            var direc = new Episode() { EpisodeId = id , AnimeId = ani, Ep = ep};
            return View(direc);
        }
        [Route("Episodeadds")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EpisodeAdds(Episode Episode, IFormFile video)
        {
            if (ModelState.IsValid)
            {
                Account account = new Account(
                    "dbpbkj4pg",
                    "718584524965961",
                    "exe6MST-mc9nLkpubAWeazM7gLI");
                Cloudinary cloudinary = new Cloudinary(account);
                cloudinary.Api.Secure = true;

                httpClient.Timeout = TimeSpan.FromMinutes(5);
                using (var hStream = video.OpenReadStream())
                {
                    var hUploadParams = new VideoUploadParams()
                    {
                        File = new FileDescription(video.FileName, hStream),
                        PublicId = "DATN/Anime/" + Episode.AnimeId.ToString() + "/Episode/ep" + Episode.Ep.ToString()+"/"+Episode.Ep.ToString()+Episode.AnimeId.ToString(),
                        Overwrite = true,
                    };
                    var hUploadResult = cloudinary.Upload(hUploadParams);
                    Episode.VideoUrl = hUploadResult.SecureUrl.ToString();
                }

                db.Episodes.Add(Episode);
                db.SaveChanges();
                return RedirectToAction("EpisodeList" , new {id = Episode.AnimeId});
            }
            return RedirectToAction("EpisodesAdd");
        }
        [Route("admin/Episodeedit")]
        [HttpGet]
        public IActionResult EpisodeEdit(string id)
        {
            var a = db.Episodes.Where(x => x.EpisodeId == id).FirstOrDefault();
            return View(a);
        }
        [Route("Episodeedit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EpisodeEdits(Episode Episode, IFormFile video)
        {
            ModelState.Remove("video");
            if (ModelState.IsValid)
            {
                if (video != null )
                {
                    Account account = new Account(
                    "dbpbkj4pg",
                    "718584524965961",
                    "exe6MST-mc9nLkpubAWeazM7gLI");
                    Cloudinary cloudinary = new Cloudinary(account);
                    cloudinary.Api.Secure = true;
                    httpClient.Timeout = TimeSpan.FromMinutes(5);
                    using (var hStream = video.OpenReadStream())
                    {
                        var hUploadParams = new VideoUploadParams()
                        {
                            File = new FileDescription(video.FileName, hStream),
                            PublicId = "DATN/Anime/" + Episode.AnimeId.ToString() + "/Episode/ep" + Episode.Ep.ToString() + "/" + Episode.Ep.ToString() + Episode.AnimeId.ToString(),
                            Overwrite = true,
                        };
                        var hUploadResult = cloudinary.Upload(hUploadParams);
                        Episode.VideoUrl = hUploadResult.SecureUrl.ToString();
                    }
                }
                db.Entry(Episode).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("EpisodeList", new { id = Episode.AnimeId });
            }
            return RedirectToAction("EpisodeEdit", new { id = Episode.EpisodeId });
        }
        [Route("Episodedelete")]
        [HttpGet]
        public IActionResult EpisodeDelete(string id)
        {
            AutoCode at = new AutoCode();
            Account account = new Account(
                "dbpbkj4pg",
                "718584524965961",
                "exe6MST-mc9nLkpubAWeazM7gLI");
            Cloudinary cloudinary = new Cloudinary(account);
            cloudinary.Api.Secure = true;
            var views = db.Views.AsNoTracking().Where(x => x.EpisodeId == id).ToList();
            if (views.Any())
            {
                db.RemoveRange(views);
            }
            var cmts = db.Comments.AsNoTracking().Where(x => x.EpisodeId == id).ToList();
            if (cmts.Any())
            {
                db.RemoveRange(cmts);
            }
            var ep = db.Episodes.AsNoTracking().Where(x=>x.EpisodeId == id).FirstOrDefault();
            var publicId = at.ExtractPathFromUrl(ep.VideoUrl);
            var deletionParams = new DeletionParams(publicId)
            {
                ResourceType = ResourceType.Video
            };
            var deletionResult = cloudinary.Destroy(deletionParams);
            db.Remove(db.Episodes.Find(id));
            db.SaveChanges();

            return RedirectToAction("EpisodeList", new { id = ep.AnimeId });
        }
    }
}
