using System;
using System.Collections.Generic;

namespace QUANLYBANHANG.Models;

public partial class Role
{
    public int RoleId { get; set; }

    public string RoleName { get; set; } = null!;

    public virtual ICollection<TaiKhoan> TaiKhoan { get; set; } = new List<TaiKhoan>();
}
