using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using DATNWEB.Models;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
namespace DATNWEB.Controllers
{
    public class LoginGGFBController : Controller
    {
        private readonly IDistributedCache _distributedCache;
        public LoginGGFBController(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }
        QlPhimAnimeContext db = new QlPhimAnimeContext();
        public async Task logingg()
        {
            await HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme, new AuthenticationProperties
            {
                RedirectUri = Url.Action("GoogleResponse")
            });
        }
        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            var openId = result.Principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var email = result.Principal.FindFirst(ClaimTypes.Email)?.Value;
            var name = result.Principal.FindFirst(ClaimTypes.Name)?.Value;
            var phone = result.Principal.FindFirst(ClaimTypes.MobilePhone)?.Value;
            var check = db.Users.FirstOrDefault(x => x.Email == email);
            string closeWindowScript = "";
            if (check == null)
            {
                // Tạo một đối tượng chứa thông tin
                AutoCode auto = new AutoCode();
                string id = "US0001";
                var maxId = db.Users.OrderByDescending(x => x.UserId).FirstOrDefault();
                if (maxId != null)
                {
                    id = auto.GenerateMa(maxId.UserId);
                }
                User a = new User
                {
                    UserId = id,
                    Username = name,
                    Password = auto.HashPassword(openId),
                    Email = email,
                    Phone = phone,
                    UserType = 0
                };
                db.Users.Add(a);
                db.SaveChanges();
                string existingClientId = _distributedCache.GetString(id);
                if (!string.IsNullOrEmpty(existingClientId))
                {
                    HttpContext.Session.Remove("SessionInfo");
                    _distributedCache.Remove(id);
                }
                string newClientId = Guid.NewGuid().ToString();
                var sessionInfo = new
                {
                    UID = id,
                    ClientId = newClientId
                };
                _distributedCache.SetString(id, JsonConvert.SerializeObject(sessionInfo));
                // Lưu thông tin vào session dưới dạng JSON
                HttpContext.Session.SetString("SessionInfo", JsonConvert.SerializeObject(sessionInfo));
            }
            else
            {
                string existingClientId = _distributedCache.GetString(check.UserId);
                if (!string.IsNullOrEmpty(existingClientId))
                {
                    HttpContext.Session.Remove("SessionInfo");
                    _distributedCache.Remove(check.UserId);
                }
                string newClientId = Guid.NewGuid().ToString();
                var sessionInfo = new
                {
                    UID = check.UserId,
                    ClientId = newClientId
                };
                _distributedCache.SetString(check.UserId, JsonConvert.SerializeObject(sessionInfo));
                // Lưu thông tin vào session dưới dạng JSON
                HttpContext.Session.SetString("SessionInfo", JsonConvert.SerializeObject(sessionInfo));
            }
            var mail = db.Users.FirstOrDefault(x => x.Email == email);
            // Trả về dữ liệu dưới dạng JSON

             closeWindowScript = "<script src='https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js'></script>" +
                           "<script src='" + Url.Content("~/pagesjs/ggfbJWT.js") + "'></script>" +
                           "<script>loginJWT('" + email + "','" + openId + "');</script>";

            // Trả về một nội dung tạm thời (temporary content) kèm theo mã JavaScript để đóng cửa sổ pop-up
            return Content(closeWindowScript, "text/html");

        }
        public async Task loginfb()
        {
            await HttpContext.ChallengeAsync(FacebookDefaults.AuthenticationScheme, new AuthenticationProperties
            {
                RedirectUri = Url.Action("FacebookResponse"),
            });
        }
        public async Task<IActionResult> FacebookResponse()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            var openId = result.Principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var email = result.Principal.FindFirst(ClaimTypes.Email)?.Value;
            var name = result.Principal.FindFirst(ClaimTypes.Name)?.Value;
            var phone = result.Principal.FindFirst(ClaimTypes.MobilePhone)?.Value;
            var check = db.Users.FirstOrDefault(x => x.Email == email);
            string closeWindowScript = "";
            if (check == null)
            {
                // Tạo một đối tượng chứa thông tin
                AutoCode auto = new AutoCode();
                string id = "US0001";
                var maxId = db.Users.OrderByDescending(x => x.UserId).FirstOrDefault();
                if (maxId != null)
                {
                    id = auto.GenerateMa(maxId.UserId);
                }
                User a = new User
                {
                    UserId = id,
                    Username = name,
                    Password = auto.HashPassword(openId),
                    Email = email,
                    Phone = phone,
                    UserType = 0
                };
                db.Users.Add(a);
                db.SaveChanges();
                string existingClientId = _distributedCache.GetString(id);
                if (!string.IsNullOrEmpty(existingClientId))
                {
                    HttpContext.Session.Remove("SessionInfo");
                    _distributedCache.Remove(id);
                }
                string newClientId = Guid.NewGuid().ToString();
                var sessionInfo = new
                {
                    UID = id,
                    ClientId = newClientId
                };
                _distributedCache.SetString(id, JsonConvert.SerializeObject(sessionInfo));
                // Lưu thông tin vào session dưới dạng JSON
                HttpContext.Session.SetString("SessionInfo", JsonConvert.SerializeObject(sessionInfo));
            }
            else
            {
                string existingClientId = _distributedCache.GetString(check.UserId);
                if (!string.IsNullOrEmpty(existingClientId))
                {
                    HttpContext.Session.Remove("SessionInfo");
                    _distributedCache.Remove(check.UserId);
                }
                string newClientId = Guid.NewGuid().ToString();
                var sessionInfo = new
                {
                    UID = check.UserId,
                    ClientId = newClientId
                };
                _distributedCache.SetString(check.UserId, JsonConvert.SerializeObject(sessionInfo));
                // Lưu thông tin vào session dưới dạng JSON
                HttpContext.Session.SetString("SessionInfo", JsonConvert.SerializeObject(sessionInfo));
            }
            var mail = db.Users.FirstOrDefault(x => x.Email == email);
            // Trả về dữ liệu dưới dạng JSON

            closeWindowScript = "<script src='https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js'></script>" +
                          "<script src='" + Url.Content("~/pagesjs/ggfbJWT.js") + "'></script>" +
                          "<script>loginJWT('" + email + "','" + openId + "');</script>";

            // Trả về một nội dung tạm thời (temporary content) kèm theo mã JavaScript để đóng cửa sổ pop-up
            return Content(closeWindowScript, "text/html");
        }
    }
}
