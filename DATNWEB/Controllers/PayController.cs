using DATNWEB.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Net.payOS.Types;
using Net.payOS;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace DATNWEB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PayController : ControllerBase
    {
        QlPhimAnimeContext db = new QlPhimAnimeContext();
        [HttpGet]
        public IActionResult Price(string id)
        {
            var lst = db.ServiceUsages.Where(x => x.PackageId == id).OrderBy(x => x.Price).ToList();
            return Ok(lst);
        }
        [HttpGet]
        [Route("infopay")]
        public IActionResult infopay(int id)
        {
            var sessionInfoJson = HttpContext.Session.GetString("SessionInfo");
            var sessionInfo = JsonConvert.DeserializeObject<dynamic>(sessionInfoJson);
            string userId = sessionInfo.UID;
            var info = db.Users.FirstOrDefault(x => x.UserId == userId);
            var pay = db.ServiceUsages.FirstOrDefault(x => x.Id == id);
            var package = db.ServicePackages.Where(x => x.PackageId == pay.PackageId).Select(x => x.PackageName).FirstOrDefault();
            var data = new
            {
                info,
                pay,
                packageName = package,
            };
            return Ok(data);
        }
        

    }
}
