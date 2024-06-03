using DATNWEB.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DATNWEB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        QlPhimAnimeContext db = new QlPhimAnimeContext();
        [HttpGet]
        public IActionResult blog(string? id)
        {
            var seall = db.Seasons
                        .Where(x => x.PostingDate < DateTime.Now)
                        .OrderByDescending(x => x.PostingDate)
                        .Select(x => x.SeasonId)
                        .ToList();
            if (id == null)
            {
                id = seall.FirstOrDefault();
            }
            var sea = db.Seasons.Find(id);
            string? sea1 = null; // Use nullable string
            string? sea2 = null; // Use nullable string
            if (sea != null)
            {
                for (int i = 0; i < seall.Count; i++)
                {
                    if (seall[i] == sea.SeasonId)
                    {
                        sea1 = i > 0 ? seall[i - 1] : null;
                        sea2 = i < seall.Count - 1 ? seall[i + 1] : null;
                        break;
                    }
                }
            }
            var animes = db.Animes
                .Where(x => x.SeasonId == id)
                .OrderByDescending(x => x.BroadcastSchedule)
                .Select(x => new { x.AnimeId, x.AnimeName, x.ImageHUrl,x.Information})
                .Take(3)
                .ToList();
            var data = new
            {
                sea,
                sea1,
                sea2,
                animes
            };
            return Ok(data);
        }
        [HttpGet]
        [Route("Test")]
        public IActionResult test()
        {
            var utc0 = DateTime.UtcNow.AddHours(7);
            var utc7 = DateTime.Now;
            var data = new
            {
                utc0,
                utc7
            };
            return Ok(data);
        }
    }
}
