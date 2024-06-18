using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DATNWEB.Models;
using Newtonsoft.Json;
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
            var sessionInfoJson = HttpContext.Session.GetString("SessionInfo");
            var sessionInfo = JsonConvert.DeserializeObject<dynamic>(sessionInfoJson);
            string userId = sessionInfo.UID;
            var p = db.ServicePackages.ToList();
            var user = db.Users.Find(userId);
            var data = new
            {
                p,
                user
            };
            return Ok(data);
        }
    }
}
