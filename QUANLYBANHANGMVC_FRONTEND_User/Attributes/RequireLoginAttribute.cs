using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace QUANLYBANHANGMVC_FRONTEND.Attributes
{
    /// <summary>
    /// Attribute để kiểm tra yêu cầu đăng nhập và role User
    /// Kiểm tra JWTToken và UserRole từ Session
    /// </summary>
    public class RequireLoginAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var token = context.HttpContext.Session.GetString("JWTToken");
            var role = context.HttpContext.Session.GetString("UserRole") ?? "";

            // Kiểm tra nếu không có token (chưa đăng nhập)
            if (string.IsNullOrEmpty(token))
            {
                // Chuyển hướng về trang đăng nhập
                context.Result = new RedirectToActionResult("Login", "Account", null);
                return;
            }

            // Kiểm tra nếu role không phải User
            if (!role.Equals("User", StringComparison.OrdinalIgnoreCase))
            {
                // Chuyển hướng về trang chủ
                context.Result = new RedirectToActionResult("Index", "Home", null);
                return;
            }

            base.OnActionExecuting(context);
        }
    }
}

