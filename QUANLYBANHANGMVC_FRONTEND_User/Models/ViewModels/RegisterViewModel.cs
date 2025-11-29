using System.ComponentModel.DataAnnotations;

namespace QUANLYBANHANGMVC_FRONTEND.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Tên đăng nhập là bắt buộc")]
        public string Username { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        [Required(ErrorMessage = "Email là bắt buộc")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mật khẩu là bắt buộc")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "Mật khẩu phải có ít nhất {2} ký tự.", MinimumLength = 6)]
        public string Password { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Mật khẩu xác nhận không khớp.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
