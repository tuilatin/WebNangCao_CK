using System;
using System.Collections.Generic;

namespace QUANLYBANHANG.Models;

public partial class ChiTietGioHang
{
    public int MaGioHang { get; set; }

    public int MaSanPham { get; set; }

    public int SoLuong { get; set; }

    public decimal DonGia { get; set; }

    public decimal? ThanhTien { get; set; }

    public DateTime? NgayThem { get; set; }

    public virtual GioHang MaGioHangNavigation { get; set; } = null!;

    public virtual SanPham MaSanPhamNavigation { get; set; } = null!;
}
