using DATNWEB.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;
namespace DATNWEB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnimeWatchingController : ControllerBase
    {
        QlPhimAnimeContext db = new QlPhimAnimeContext();
        [HttpGet]
        public IActionResult animewatch(string aid, int? ep)
        {
            var user = db.Users.Find(HttpContext.Session.GetString("UID"));
            var ani = db.Animes.Find(aid);
            var check = db.Episodes.Where(x => x.Ep == ep && x.AnimeId == aid).Select(x => x.PostingDate).FirstOrDefault();
            if (user.UserType == 0 && check < DateTime.Now.AddDays(7) && check > DateTime.Now)
            {
                return BadRequest("Bạn không phải là VIP");
            }
            if (user.UserType == 0 && ani.Permission == 0 && ep != 0)
            {
                return BadRequest("Bạn không phải là VIP");
            }
            if (user.UserType != 2 && check > DateTime.Now.AddDays(7))
            {
                return BadRequest("Bạn không phải là SVIP");
            }
            var eps = db.Episodes
                        .Where(x => x.AnimeId == aid)
                        .OrderBy(x => x.Ep)
                        .Select(x => new { Ep = x.Ep, PostingDate = x.PostingDate })
                        .ToList();

            ep = ep ?? 0;
            var epi = db.Episodes.Where(x => x.AnimeId == aid && x.Ep == ep).FirstOrDefault();
            var viewDuration = db.Views
                    .Where(x => x.EpisodeId == epi.EpisodeId && x.UserId == HttpContext.Session.GetString("UID"))
                    .OrderByDescending(x => x.ViewDate) // Sắp xếp giảm dần theo ngày xem
                    .Select(x => x.Duration) // Chỉ lấy thời lượng
                    .FirstOrDefault();

            // Convert the TimeSpan to seconds
            int view = viewDuration.HasValue ? (int)viewDuration.Value.TotalSeconds : 0;

            var detail = new
            {
                epi.AnimeId,
                ani.BackgroundImageUrl,
                epi.VideoUrl,
                epi.EpisodeId,
                epi.Title,
                e = eps,
                epside = ep,
                ani.Permission,
                view
            };            
            return Ok(detail);
        }
        [Route("getcmt")]
        [HttpGet]
        public IActionResult getcmt(string eid, int? page)
        {
            const int pageSize = 6;
            var cmts = db.Comments.Where(x => x.EpisodeId == eid).ToList();
            var cmtss = (from r in cmts
                         select new
                         {
                             r.Id,
                             r.EpisodeId,
                             r.CommentDate,
                             r.Comment1,
                             User = db.Users.Where(x => x.UserId == r.UserId).Select(x => x.Username).FirstOrDefault()
                         }).OrderByDescending(x => x.CommentDate).ToList();
            var totalCMT = cmtss.Count;
            var totalPages = (int)Math.Ceiling(totalCMT / (double)pageSize);
            int pageNumber = (page ?? 1);
            var pagedCMT = cmtss.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            var paginationInfo = new
            {
                TotalPages = totalPages,
                CurrentPage = pageNumber
            };
            return Ok(new { PagedCMT = pagedCMT, PaginationInfo = paginationInfo });
        }
        [Route("addcmt")]
        [HttpPost]
        public IActionResult addcmt([FromBody] Comment cmt)
        {
            db.Comments.Add(cmt);
            db.SaveChanges();
            return Ok(cmt);
        }
        [Route("addview")]
        [HttpPost]
        public IActionResult addview([FromBody] View view)
        {
            var v1 = db.Views.Where(x => x.EpisodeId == view.EpisodeId & x.UserId == view.UserId).OrderByDescending(x => x.ViewDate).FirstOrDefault();
            if (v1 != null)
            {
                // Tính sự khác biệt về thời gian giữa ViewDate của v1 và view
                TimeSpan? timeDifference = v1.ViewDate - view.ViewDate ?? TimeSpan.Zero;
                if (v1.IsView == 0 || timeDifference?.TotalMinutes < 20)
                {
                    v1.ViewDate = view.ViewDate;
                    v1.Duration = view.Duration;
                    v1.IsView = view.IsView;
                    db.Views.Update(v1);
                    db.SaveChanges();
                    return Ok(v1);
                }
            }
            db.Views.Add(view);
            db.SaveChanges();
            return Ok(view);
        }
    }
}
