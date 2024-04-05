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
        public IActionResult animewatch(string aid,int? ep)
        {     
            var ani = db.Animes.Find(aid);
            var eps = db.Episodes.Where(x => x.AnimeId == aid).OrderBy(x=>x.Ep).Select(x=>x.Ep).ToList();
            ep = ep ?? 0;
            var epi = db.Episodes.Where(x => x.AnimeId == aid && x.Ep == ep).FirstOrDefault();
            var detail = new
            {
                epi.AnimeId,
                ani.BackgroundImageUrl,
                epi.VideoUrl,
                epi.EpisodeId,
                epi.Title,
                e = eps,
                epside = ep
            };
            return Ok(detail);
        }
        [Route("getcmt")]
        [HttpGet]
        public IActionResult getcmt(string eid,int? page)
        {
            const int pageSize = 6;
            var cmts = db.Comments.Where(x =>x.EpisodeId == eid).ToList();
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
    }
}
