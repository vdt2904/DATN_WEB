using DATNWEB.helpter;
using DATNWEB.Models;
using DATNWEB.Service;
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
        private readonly IMailService mailService;
        QlPhimAnimeContext db = new QlPhimAnimeContext();
        public AccountController(IConfiguration config,IMailService service)
        {
            _config = config;
            this.mailService = service;
        }
        [HttpPost]
        public IActionResult Login([FromBody] Login  login)
        {
            if (login.Mail == ""||login.Pass == "")
            {
                return BadRequest("Vui lòng nhập đầy đủ");
            }
            AutoCode a = new AutoCode();
            var user = db.Users.FirstOrDefault(x => (x.Username == login.Mail || x.Email == login.Mail || x.Phone == login.Mail) && x.Password == a.HashPassword(login.Pass));
            if (user != null)
            {
                HttpContext.Session.SetString("UID", user.UserId);
                var token = GenerateToken(user);
                return Ok(new { token , user.UserId});
            }
            return BadRequest("Tài khoản,Mật khẩu không đúng");
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
        [Route("Register")]
        [HttpPost]
        public IActionResult Register([FromBody] Register register)
        {
            if(register.Code == "" || register.Username == "" || register.Mail == "")
            {
                return BadRequest("Vui lòng nhập đầy đủ thông tin");
            }
            var codes = db.CodeRegisters.Where(x => x.Email == register.Mail && x.Token == register.Code).OrderByDescending(x => x.SentDate).FirstOrDefault();
            if(codes!= null)
            {
                if (codes.SentDate < DateTime.Now.AddMinutes(-5))
                {
                    return BadRequest("Mã đã hết hạn");
                }
                AutoCode at = new AutoCode();
                var uid_max = db.Users.OrderByDescending(x => x.UserId).FirstOrDefault();
                string id = "AN0001";
                if (uid_max != null)
                {
                    AutoCode a = new AutoCode();
                    id = a.GenerateMa(uid_max.UserId);
                }
                User user = new User
                {
                    UserId = id,
                    Username = register.Username,
                    Password = at.HashPassword(register.Pass),
                    Email = register.Mail,
                    Phone = null,
                    UserType = 0
                };
                db.Users.Add(user);
                db.SaveChanges();
                HttpContext.Session.SetString("UID", user.UserId);
                return Ok();
            }
            else
            {
                return BadRequest("Mã OTP sai");
            }
        }
        [HttpPost]
        [Route("sendmail")]
        public async Task<IActionResult> sendmail([FromBody] MailRequest mailrequest)
        {
            try{
                if(mailrequest.ToEmail == "")
                {
                    return BadRequest("Vui lòng nhập email");
                }
                if(mailrequest.Num == 1)
                {
                    var u = db.Users.Where(x => x.Email == mailrequest.ToEmail).FirstOrDefault();
                    if(u != null)
                    {
                        return BadRequest("Email đã tồn tại!");
                    }
                    CodeRegister c = new CodeRegister
                    {
                        Email = mailrequest.ToEmail,
                        Token = mailrequest.Code,
                        SentDate = DateTime.Now
                    };
                    db.CodeRegisters.Add(c);
                    db.SaveChanges();
                }else if(mailrequest.Num == 2)
                {
                    var a = db.Users.FirstOrDefault(x => x.Email == mailrequest.ToEmail);
                    if(a != null)
                    {
                        var b = db.PasswordResetRequests.FirstOrDefault(x => x.UserId == a.UserId);
                        if(b!= null) { 
                            if(b.RequestDate < DateTime.Now.AddMinutes(-1))
                            {
                                PasswordResetRequest p = new PasswordResetRequest
                                {
                                    UserId = a.UserId,
                                    Token = mailrequest.Code,
                                    RequestDate= DateTime.Now
                                };
                                db.PasswordResetRequests.Add(p);
                                db.SaveChanges();
                            }
                        }
                        else
                        {
                            PasswordResetRequest p = new PasswordResetRequest
                            {
                                UserId = a.UserId,
                                Token = mailrequest.Code,
                                RequestDate = DateTime.Now
                            };
                            db.PasswordResetRequests.Add(p);
                            db.SaveChanges();
                        }
                    }
                    else
                    {
                        return BadRequest("Email không tồn tại trong hệ thống!");
                    }
                }
                await mailService.SendEmailAsync(mailrequest);
                return Ok(mailrequest);
            }
            catch(Exception ex){
                throw;
            }
        }

    }
}
