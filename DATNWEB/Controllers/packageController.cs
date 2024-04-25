using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DATNWEB.Models;
namespace DATNWEB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class packageController : ControllerBase
    {
        QlPhimAnimeContext db = new QlPhimAnimeContext();
        [HttpGet]
        public IActionResult package()
        {
            var p = db.ServicePackages.ToList();
            return Ok(p);
        }
    }
}
