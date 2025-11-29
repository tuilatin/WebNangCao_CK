using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Json;
using QUANLYBANHANGMVC_FRONTEND.Models.ViewModels;
using System.Text.Json;

namespace QUANLYBANHANGMVC_FRONTEND.Controllers
{
    public class AccountController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AccountController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        private HttpClient CreateClient()
        {
            return _httpClientFactory.CreateClient("BackendApi");
        }

        // GET: Account/Login
        public IActionResult Login()
        {
            // Kiểm tra nếu có thông báo lỗi từ session
            var errorMessage = HttpContext.Session.GetString("ErrorMessage");
            if (!string.IsNullOrEmpty(errorMessage))
            {
                TempData["ErrorMessage"] = errorMessage;
                HttpContext.Session.Remove("ErrorMessage");
            }
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var client = CreateClient();
            var loginRequest = new
            {
                Username = model.Username,
                Password = model.Password
            };

            var response = await client.PostAsJsonAsync("Auth/Login", loginRequest);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<JsonElement>();

                // LẤY TOKEN
                if (result.TryGetProperty("token", out var tokenElement))
                {
                    var token = tokenElement.GetString();
                    HttpContext.Session.SetString("JWTToken", token);
                }

                // LẤY USERNAME
                if (result.TryGetProperty("username", out var usernameElement))
                {
                    HttpContext.Session.SetString("Username", usernameElement.GetString() ?? "");
                }

                // LẤY ROLE (QUAN TRỌNG)
                // API trả về "role" (số ít) chứ không phải "roles" (số nhiều)
                if (result.TryGetProperty("role", out var roleElement))
                {
                    var roleValue = roleElement.GetString();
                    HttpContext.Session.SetString("UserRole", roleValue ?? "");
                }
                // Fallback: Nếu không có "role", thử tìm "roles" (số nhiều) để tương thích
                else if (result.TryGetProperty("roles", out var rolesElement))
                {
                    var roles = rolesElement.EnumerateArray()
                                            .Select(r => r.GetString())
                                            .ToList();

                    // Lưu role đầu tiên vào session
                    HttpContext.Session.SetString("UserRole", roles.FirstOrDefault() ?? "");
                }

                TempData["Success"] = "Đăng nhập thành công!";

                // CHUYỂN HƯỚNG THEO ROLE
                var role = HttpContext.Session.GetString("UserRole");

                // Nếu là Admin hoặc Staff, chuyển đến trang SanPham/Index
                if (role == "Admin" || role == "Staff")
                {
                    return RedirectToAction("Index", "SanPham");
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng.");
            return View(model);
        }

        // GET: Account/Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var client = CreateClient();
            var registerRequest = new
            {
                Username = model.Username,
                Email = model.Email,
                Password = model.Password
            };

            var response = await client.PostAsJsonAsync("Auth/Register", registerRequest);

            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Đăng ký thành công! Vui lòng đăng nhập.";
                return RedirectToAction("Login");
            }

            // Đọc lỗi từ API
            var errorContent = await response.Content.ReadAsStringAsync();
            try
            {
                var errorResult = await response.Content.ReadFromJsonAsync<JsonElement>();
                if (errorResult.TryGetProperty("Message", out var messageElement))
                {
                    ModelState.AddModelError("", messageElement.GetString() ?? "Đăng ký thất bại.");
                }
                else
                {
                    ModelState.AddModelError("", "Đăng ký thất bại. Vui lòng thử lại.");
                }
            }
            catch
            {
                ModelState.AddModelError("", "Đăng ký thất bại. Vui lòng thử lại.");
            }

            return View(model);
        }

        // POST: Account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            TempData["Success"] = "Đăng xuất thành công!";
            return RedirectToAction("Index", "Home");
        }
    }
}