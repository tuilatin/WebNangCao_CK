using System;
using System.Collections.Generic;

namespace QUANLYBANHANG.Models;

public partial class DonHang
{
    public int MaDonHang { get; set; }

    public int MaKhachHang { get; set; }

    public DateTime? NgayDatHang { get; set; }

    public decimal TongTien { get; set; }

    public string? TrangThai { get; set; }

    public int MaPhuongThuc { get; set; }

    public string? NguoiDuyet { get; set; }

    public virtual ICollection<ChiTietDonHang> ChiTietDonHang { get; set; } = new List<ChiTietDonHang>();

    public virtual KhachHang MaKhachHangNavigation { get; set; } = null!;

    public virtual PhuongThucThanhToan MaPhuongThucNavigation { get; set; } = null!;
}
