using System;
using System.Collections.Generic;

namespace QUANLYBANHANG.Models;

public partial class PhuongThucThanhToan
{
    public int MaPhuongThuc { get; set; }

    public string TenPhuongThuc { get; set; } = null!;

    public decimal? PhiGiaoDich { get; set; }

    public virtual ICollection<DonHang> DonHang { get; set; } = new List<DonHang>();
}
