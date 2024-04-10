using DATNWEB.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DATNWEB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class infouserController : ControllerBase
    {
        QlPhimAnimeContext db = new QlPhimAnimeContext();
        [HttpGet]
        public IActionResult infouser()
        {
            var a = HttpContext.Session.GetString("UID");
            var info = db.Users.Find(a);
            return Ok(info);
        }
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            try
            {
                HttpContext.Session.Remove("UID");
                return Ok("Logout successful");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error logging out");
            }
        }

    }
}
