using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QUANLYBANHANG.Models;

namespace QUANLYBANHANG.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GioHangController : ControllerBase
    {
        private readonly AppDbContext _context;

        public GioHangController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/GioHang
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GioHang>>> GetGioHang()
        {
            return await _context.GioHang.ToListAsync();
        }

        // GET: api/GioHang/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GioHang>> GetGioHang(int id)
        {
            var gioHang = await _context.GioHang.FindAsync(id);

            if (gioHang == null)
            {
                return NotFound();
            }

            return gioHang;
        }

        // PUT: api/GioHang/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGioHang(int id, GioHang gioHang)
        {
            if (id != gioHang.MaGioHang)
            {
                return BadRequest();
            }

            _context.Entry(gioHang).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GioHangExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/GioHang
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<GioHang>> PostGioHang(GioHang gioHang)
        {
            _context.GioHang.Add(gioHang);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGioHang", new { id = gioHang.MaGioHang }, gioHang);
        }

        // DELETE: api/GioHang/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGioHang(int id)
        {
            var gioHang = await _context.GioHang.FindAsync(id);
            if (gioHang == null)
            {
                return NotFound();
            }

            _context.GioHang.Remove(gioHang);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool GioHangExists(int id)
        {
            return _context.GioHang.Any(e => e.MaGioHang == id);
        }
    }
}
