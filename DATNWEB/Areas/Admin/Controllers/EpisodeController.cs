﻿using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using DATNWEB.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList;
using Microsoft.AspNetCore.Mvc.Rendering;
using DATNWEB.Models.Authentication;
using DATNWEB.Hubs;
using Microsoft.AspNetCore.SignalR;
using DATNWEB.Models.ViewModel;
using Hangfire;

namespace DATNWEB.Areas.Admin.Controllers
{
    
    [Area("admin")]
    public class EpisodeController : Controller
    {
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly IBackgroundJobClient _backgroundJobClient;
        public EpisodeController(IHubContext<NotificationHub> hubContext, IBackgroundJobClient backgroundJobClient)
        {
            _hubContext = hubContext;
            _backgroundJobClient = backgroundJobClient;
        }
        private HttpClient httpClient = new HttpClient();
        QlPhimAnimeContext db = new QlPhimAnimeContext();
        [AuthForAdmin]
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
        [AuthForAdmin]
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
        [AuthForAdmin]
        [Route("Episodeadds")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EpisodesAdd(Episode Episode, IFormFile video)
        {
            if (ModelState.IsValid)
            {
                Account account = new Account(
                    "dbpbkj4pg",
                    "718584524965961",
                    "exe6MST-mc9nLkpubAWeazM7gLI");
                Cloudinary cloudinary = new Cloudinary(account);
                cloudinary.Api.Secure = true;

                httpClient.Timeout = TimeSpan.FromMinutes(10);
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
                var b = db.Animes.Find(Episode.AnimeId);
                NotificationView a = new NotificationView
                {
                    AnimeId = Episode.AnimeId,
                    AnimeName = b.AnimeName,
                    ImageVUrl = b.ImageVUrl,
                    Episode = Episode.Ep,
                    PostingDate = Episode.PostingDate,
                    Title = Episode.Title,
                    Permission = b.Permission,
                };
                db.Episodes.Add(Episode);
                db.SaveChanges();
                _backgroundJobClient.Schedule<EpisodeController>(x => x.AddNewEpisodes(a), Episode.PostingDate);
                //await AddNewEpisode(a);
                return RedirectToAction("EpisodeList" , new {id = Episode.AnimeId});
            }
            return View(Episode);
        }
        public void AddNewEpisodes(NotificationView a)
        {
             AddNewEpisode(a);           
        }

        public async Task AddNewEpisode(NotificationView newEpisode)
        {
            // Thêm tập phim mới vào cơ sở dữ liệu
            // ...
            var a = newEpisode;
            // Gửi thông báo đến các client
            await _hubContext.Clients.All.SendAsync("ReceiveNewEpisodeNotification", newEpisode);
        }
        [AuthForAdmin]
        [Route("admin/Episodeedit")]
        [HttpGet]
        public IActionResult EpisodeEdit(string id)
        {
            var a = db.Episodes.Where(x => x.EpisodeId == id).FirstOrDefault();
            return View(a);
        }
        [AuthForAdmin]
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
        [AuthForAdmin]
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
