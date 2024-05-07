using DATNWEB.helpter;
using DATNWEB.Models;
using DATNWEB.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Net.payOS;
using Net.payOS.Types;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace DATNWEB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class infouserController : ControllerBase
    {
        private readonly PayOS _payOS;
        public infouserController(PayOS payos)
        {
            _payOS = payos;
        }
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
        [HttpPost("updatesdt")]
        public async Task<IActionResult> updatesdt([FromBody] string phoneNumber)
        {
            try
            {
                var userId = HttpContext.Session.GetString("UID");
                if (userId == null)
                {
                    return NotFound("Không tìm thấy người dùng");
                }
                var user = await db.Users.FindAsync(userId);
                if (user == null)
                {
                    return NotFound("Không tìm thấy người dùng");
                }
                user.Phone = phoneNumber;
                await db.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi: {ex.Message}");
            }
        }
        [HttpGet("checkotp")]
        public IActionResult CheckOTP(string otp, string mail)
        {
            var user = db.Users.FirstOrDefault(x => x.Email == mail);
            if (user == null)
            {
                return BadRequest("Email không tồn tại trong hệ thống!");
            }
            // Kiểm tra xem OTP có tồn tại trong bảng passforget hay không
            var a = db.PasswordResetRequests.FirstOrDefault(x => x.UserId == user.UserId && x.Token == otp);
            if (a == null)
            {
                return BadRequest("Sai mã xác nhận!");
            }
            if (a.RequestDate < DateTime.Now.AddMinutes(-5))
            {
                return BadRequest("Sai mã xác nhận!");
            }
            return Ok("Mã hợp lệ");
        }
        [HttpPost("updatepass")]
        public async Task<IActionResult> updatepass([FromBody] Login a)
        {
            try
            {
                var userId = db.Users.FirstOrDefault(x => x.Email == a.Mail);
                if (userId == null)
                {
                    return NotFound("Email không tồn tại!");
                }
                AutoCode at = new AutoCode();
                userId.Password = at.HashPassword(a.Pass);
                db.Users.Update(userId);
                await db.SaveChangesAsync();
                return Ok("Đổi mật khẩu thành công");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi: {ex.Message}");
            }
        }
        [HttpGet("/home/infousers")]
        public IActionResult ProcessPayment(
        [FromQuery(Name = "code")] string code,
        [FromQuery(Name = "id")] string id,
        [FromQuery(Name = "cancel")] bool cancel,
        [FromQuery(Name = "status")] string status,
        [FromQuery(Name = "orderCode")] int orderCode)
        {
            try
            {
                // Xử lý các tham số từ URL tại đây
                if (status == "PAID")
                {
                    // Xử lý thanh toán đã thành công
                    var bill = db.Bills.Where(x => x.Createat > DateTime.Now.AddMinutes(-10)).ToList();
                    foreach(var a in bill)
                    {
                        var p = db.ServiceUsages.Find(a.Ids);
                        if (HttpContext.Session.GetString("UID") == a.Userid && a.Id == id) {
                            UserSubscription us = new UserSubscription
                            {
                                UserId = a.Userid,
                                PackageId = p.PackageId,
                                SubscriptionDate= DateTime.Now,
                                ExpirationDate= DateTime.Now.AddMonths(p.UsedTime),
                            };
                            db.UserSubscriptions.Add(us);
                            db.SaveChanges();
                            db.Bills.Remove(a);
                            db.SaveChanges();
                        } ;
                    }
                    // Ví dụ:
                    return Redirect("/home/infouser");

                }
                else
                {
                    // Xử lý khi thanh toán không thành công
                    // Ví dụ:
                    return BadRequest("Payment failed");
                }
            }
            catch (Exception ex)
            {
                // Xử lý các lỗi nếu cần
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}
