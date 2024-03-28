using DATNWEB.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DATNWEB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private IConfiguration _config;
        QlPhimAnimeContext db = new QlPhimAnimeContext();
        public AccountController(IConfiguration config)
        {
            _config = config;
        }
        [HttpPost]
        public IActionResult Login([FromBody] Login  login)
        {
            AutoCode a = new AutoCode();
            var user = db.Users.FirstOrDefault(x => (x.Username == login.Username || x.Email == login.Username || x.Phone == login.Username) && x.Password == a.HashPassword(login.Pass));
            if (user != null)
            {
                HttpContext.Session.SetString("UID", user.UserId);
                var token = GenerateToken(user);
                return Ok(new { token , user.UserId});
            }
            return BadRequest("Invalid user");
        }
        public string GenerateToken(User user)
        {
            var securityKey = Encoding.UTF8.GetBytes(_config["Jwt:Secret"]);
            var credentials = new SigningCredentials(new SymmetricSecurityKey(securityKey), SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(ClaimTypes.Name, user.UserId.ToString()),
            new Claim(ClaimTypes.Name, user.Username)
            };

            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Issuer"],
                claims,
                expires: DateTime.Now.AddDays(7),
                signingCredentials: credentials
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
