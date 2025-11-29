using Microsoft.AspNetCore.Mvc;
using QUANLYBANHANGMVC_FRONTEND.Attributes;
using QUANLYBANHANGMVC_FRONTEND.Extensions;

namespace QUANLYBANHANGMVC_FRONTEND.Controllers
{
    public class SanPhamController : Controller
    {
        // GET: SanPham/Index
        public IActionResult Index()
        {
            ViewData["Title"] = "Danh sách Sản phẩm";
            
            // Kiểm tra đăng nhập và role
            var token = HttpContext.Session.GetString("JWTToken");
            var role = HttpContext.Session.GetString("UserRole");
            
            // Nếu là Admin hoặc Staff, dùng Admin layout (cần kiểm tra quyền nghiêm ngặt)
            if (role == "Admin" || role == "Staff")
            {
                // Kiểm tra lại quyền trước khi hiển thị Admin view
                if (string.IsNullOrEmpty(token))
                {
                    TempData["ErrorMessage"] = "Vui lòng đăng nhập để truy cập.";
                    return RedirectToAction("Login", "Account");
                }
                
                // Đảm bảo chỉ Admin và Staff mới được xem Admin view
                // Chặn KhachHang, NhanVien, User
                if (role != "Admin" && role != "Staff")
                {
                    TempData["ErrorMessage"] = "Bạn không có quyền truy cập trang quản trị.";
                    return RedirectToAction("Login", "Account");
                }
                
                return View("~/Views/Admin/SanPham/Index.cshtml");
            }
            
            // User thường (KhachHang, NhanVien, User) hoặc chưa đăng nhập dùng layout thường
            // Họ chỉ xem được view thường, không xem được Admin view
            return View();
        }

        // GET: SanPham/Details/5 - Tất cả user đều có thể xem
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewData["Title"] = "Chi tiết Sản phẩm";
            return View();
        }

        // GET: SanPham/Create - Chỉ Admin và Staff
        [AdminOnly]
        public IActionResult Create()
        {
            ViewData["Title"] = "Thêm sản phẩm mới";
            return View();
        }

        // POST: SanPham/Create - Chỉ Admin và Staff
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AdminOnly]
        public IActionResult Create(object model)
        {
            // Logic tạo sản phẩm sẽ được thêm sau
            return RedirectToAction(nameof(Index));
        }

        // GET: SanPham/Edit/5 - Chỉ Admin và Staff
        [AdminOnly]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewData["Title"] = "Chỉnh sửa Sản phẩm";
            return View();
        }

        // POST: SanPham/Edit/5 - Chỉ Admin và Staff
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AdminOnly]
        public IActionResult Edit(int id, object model)
        {
            // Logic chỉnh sửa sản phẩm sẽ được thêm sau
            return RedirectToAction(nameof(Index));
        }

        // GET: SanPham/Delete/5 - Chỉ Admin và Staff
        [AdminOnly]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewData["Title"] = "Xóa Sản phẩm";
            return View();
        }

        // POST: SanPham/Delete/5 - Chỉ Admin và Staff
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [AdminOnly]
        public IActionResult DeleteConfirmed(int id)
        {
            // Logic xóa sản phẩm sẽ được thêm sau
            return RedirectToAction(nameof(Index));
        }
    }
}

