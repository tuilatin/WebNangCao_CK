using System;
using System.Collections.Generic;

namespace QUANLYBANHANG.Models;

public partial class SanPham
{
    public int MaSanPham { get; set; }

    public string TenSanPham { get; set; } = null!;

    public decimal DonGia { get; set; }

    public int? SoLuong { get; set; }

    public string? HinhAnh { get; set; }

    public string? MoTa { get; set; }

    public DateTime? NgayTao { get; set; }

    public DateTime? NgayCapNhat { get; set; }

    public int? MaDanhMuc { get; set; }

    public virtual ICollection<ChiTietDonHang> ChiTietDonHang { get; set; } = new List<ChiTietDonHang>();

    public virtual ICollection<ChiTietGioHang> ChiTietGioHang { get; set; } = new List<ChiTietGioHang>();

    public virtual DanhMuc? MaDanhMucNavigation { get; set; }
}
