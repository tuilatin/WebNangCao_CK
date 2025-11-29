using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace QUANLYBANHANGMVC_FRONTEND.Attributes
{
    /// <summary>
    /// Attribute để kiểm tra chỉ Admin và Staff mới được truy cập
    /// Không cho phép: KhachHang, NhanVien, User hoặc chưa đăng nhập
    /// Kiểm tra role từ Session
    /// </summary>
    public class AdminOnlyAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // Kiểm tra token đăng nhập
            var token = context.HttpContext.Session.GetString("JWTToken");
            var role = context.HttpContext.Session.GetString("UserRole");

            // Nếu chưa đăng nhập (không có token) hoặc không có role
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(role))
            {
                context.Result = new RedirectToActionResult("Login", "Account", null);
                return;
            }

            // Chỉ cho phép Admin và Staff
            // Không cho phép: KhachHang, NhanVien, User hoặc các role khác
            if (role != "Admin" && role != "Staff")
            {
                // Chuyển hướng về trang đăng nhập với thông báo
                context.HttpContext.Session.SetString("ErrorMessage", "Bạn không có quyền truy cập trang này.");
                context.Result = new RedirectToActionResult("Login", "Account", null);
                return;
            }

            base.OnActionExecuting(context);
        }
    }
}

