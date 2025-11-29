using System;
using System.Collections.Generic;

namespace QUANLYBANHANG.Models;

public partial class GioHang
{
    public int MaGioHang { get; set; }

    public int MaKhachHang { get; set; }

    public DateTime? NgayTao { get; set; }

    public DateTime? NgayCapNhat { get; set; }

    public string? TrangThai { get; set; }

    public virtual ICollection<ChiTietGioHang> ChiTietGioHang { get; set; } = new List<ChiTietGioHang>();

    public virtual KhachHang MaKhachHangNavigation { get; set; } = null!;
}
