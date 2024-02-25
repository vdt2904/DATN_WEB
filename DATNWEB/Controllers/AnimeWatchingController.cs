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
        public IActionResult cmt(string eid,int? page)
        {
            const int pageSize = 1;
            var cmts = db.Comments.Where(x =>x.EpisodeId == eid).ToList();
            int pageNumber = (page ?? 1);
            var pageCmts = cmts.ToPagedList(pageNumber, pageSize);
            return Ok(pageCmts);
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
