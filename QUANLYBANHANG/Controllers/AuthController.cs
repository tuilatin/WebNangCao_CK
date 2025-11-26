using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using QUANLYBANHANG.Models;
using QUANLYBANHANG.Models.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace QUANLYBANHANG.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // POST: api/Auth/Login
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Tìm tài khoản theo username
            var taiKhoan = await _context.TaiKhoan
                .Include(t => t.Role)
                .FirstOrDefaultAsync(t => t.Username == model.Username);

            if (taiKhoan == null)
            {
                return Unauthorized(new { Message = "Tên đăng nhập hoặc mật khẩu không đúng." });
            }

            // Kiểm tra mật khẩu (giả sử password được lưu plain text hoặc đã hash)
            // Nếu password đã được hash, sử dụng BCrypt.Net để verify
            // Ở đây tôi giả sử password được lưu plain text để đơn giản
            // Trong production, nên hash password khi đăng ký và verify khi đăng nhập
            if (taiKhoan.Password != model.Password)
            {
                // Nếu có BCrypt, sử dụng: BCrypt.Net.BCrypt.Verify(model.Password, taiKhoan.Password)
                return Unauthorized(new { Message = "Tên đăng nhập hoặc mật khẩu không đúng." });
            }

            // Lấy role name
            var roleName = taiKhoan.Role?.RoleName ?? "User";

            // Tạo JWT token
            var token = GenerateJwtToken(taiKhoan.Username, roleName);

            return Ok(new
            {
                token = token,
                username = taiKhoan.Username,
                role = roleName
            });
        }

        // POST: api/Auth/Register
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Kiểm tra username đã tồn tại chưa
            if (await _context.TaiKhoan.AnyAsync(t => t.Username == model.Username))
            {
                return BadRequest(new { Message = "Tên đăng nhập đã tồn tại." });
            }

            // Kiểm tra email đã tồn tại chưa (nếu có trong KhachHang)
            if (!string.IsNullOrEmpty(model.Email) && 
                await _context.KhachHang.AnyAsync(k => k.Email == model.Email))
            {
                return BadRequest(new { Message = "Email đã được sử dụng." });
            }

            // Tìm role "User" mặc định (RoleId = 3 hoặc tìm theo RoleName)
            var userRole = await _context.Role.FirstOrDefaultAsync(r => r.RoleName == "KhachHang");
            if (userRole == null)
            {
                // Nếu không có role User, tạo mới hoặc lấy role đầu tiên
                userRole = await _context.Role.FirstOrDefaultAsync();
                if (userRole == null)
                {
                    return BadRequest(new { Message = "Không tìm thấy role phù hợp." });
                }
            }

            // Tạo KhachHang mới
            var khachHang = new KhachHang
            {
                HoTen = model.Username, // Tạm thời dùng username làm tên
                Email = model.Email,
            };

            _context.KhachHang.Add(khachHang);
            await _context.SaveChangesAsync();

            // Tạo TaiKhoan mới
            // Lưu ý: Trong production, nên hash password bằng BCrypt
            // var hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.Password);
            var taiKhoan = new TaiKhoan
            {
                Username = model.Username,
                Password = model.Password, // Nên hash password trước khi lưu
                RoleId = userRole.RoleId,
                MaKhachHang = khachHang.MaKhachHang
            };

            _context.TaiKhoan.Add(taiKhoan);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Đăng ký thành công!" });
        }

        private string GenerateJwtToken(string username, string role)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, role),
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(24),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
