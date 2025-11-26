using System;
using System.Collections.Generic;

namespace QUANLYBANHANG.Models;

public partial class KhachHang
{
    public int MaKhachHang { get; set; }

    public string HoTen { get; set; } = null!;

    public string? Email { get; set; }

    public DateOnly? NgaySinh { get; set; }

    public string? GioiTinh { get; set; }

    public string? DiaChi { get; set; }

    public DateTime? NgayDangKy { get; set; }

    public virtual ICollection<DonHang> DonHang { get; set; } = new List<DonHang>();

    public virtual GioHang? GioHang { get; set; }

    public virtual ICollection<TaiKhoan> TaiKhoan { get; set; } = new List<TaiKhoan>();
}
