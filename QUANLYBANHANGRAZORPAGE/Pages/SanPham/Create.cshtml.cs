using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using QUANLYBANHANG.Models;

namespace QUANLYBANHANGRAZORPAGE.Pages.SanPham
{
    public class CreateModel : PageModel
    {
        private readonly QUANLYBANHANG.Models.AppDbContext _context;

        public CreateModel(QUANLYBANHANG.Models.AppDbContext context)
        {
            _context = context;
        }

        public IList<TbSanpham> TbSanpham { get;set; } = default!;

        public async Task OnGetAsync()
        {
            TbSanpham = await _context.TbSanpham
                .Include(t => t.MadanhmucNavigation).ToListAsync();
        }
    }
}
