using DATNWEB.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DATNWEB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogDetailController : ControllerBase
    {
        QlPhimAnimeContext db = new QlPhimAnimeContext();
        [HttpGet]
        public IActionResult blogdetail(string id) {
            var animes = db.Animes.Where(x=>x.SeasonId == id).Select(x=> new {x.AnimeId , x.AnimeName ,x.ImageHUrl,x.BroadcastSchedule}).ToList();
            return Ok(animes);
        }
    }
}
