using System;
using System.Collections.Generic;

namespace QUANLYBANHANG.Models;

public partial class TaiKhoan
{
    public int MaTaiKhoan { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int? RoleId { get; set; }

    public int? MaKhachHang { get; set; }

    public virtual KhachHang? MaKhachHangNavigation { get; set; }

    public virtual Role? Role { get; set; }
}
