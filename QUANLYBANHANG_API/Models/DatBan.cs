using System;
using System.Collections.Generic;

namespace QUANLYBANHANG.Models;

public partial class DatBan
{
    public int MaDatBan { get; set; }

    public string HoTen { get; set; } = null!;

    public string? Email { get; set; }

    public string SoDienThoai { get; set; } = null!;

    public string ChiNhanh { get; set; } = null!;

    public DateTime ThoiGianDen { get; set; }

    public string? NoiDung { get; set; }

    public string? TrangThai { get; set; }

    public DateTime? NgayDat { get; set; }
}
