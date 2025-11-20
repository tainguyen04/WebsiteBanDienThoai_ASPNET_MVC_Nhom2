using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QLCHBanDienThoaiMoi.Data;
using QLCHBanDienThoaiMoi.Models;

namespace QLCHBanDienThoaiMoi.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HoaDonBansController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HoaDonBansController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/HoaDonBans
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.HoaDonBan.Include(h => h.KhachHang).Include(h => h.NhanVien);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Admin/HoaDonBans/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hoaDonBan = await _context.HoaDonBan
                .Include(h => h.KhachHang)
                .Include(h => h.NhanVien)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (hoaDonBan == null)
            {
                return NotFound();
            }

            return View(hoaDonBan);
        }

        // GET: Admin/HoaDonBans/Create
        public IActionResult Create()
        {
            ViewData["KhachHangId"] = new SelectList(_context.KhachHang, "Id", "Id");
            ViewData["NhanVienId"] = new SelectList(_context.NhanVien, "Id", "Id");
            return View();
        }

        // POST: Admin/HoaDonBans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NgayBan,KhachHangId,NhanVienId,DiaChiNhanHang,TongTien,PhuongThucThanhToan,TrangThai")] HoaDonBan hoaDonBan)
        {
            if (ModelState.IsValid)
            {
                _context.Add(hoaDonBan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["KhachHangId"] = new SelectList(_context.KhachHang, "Id", "Id", hoaDonBan.KhachHangId);
            ViewData["NhanVienId"] = new SelectList(_context.NhanVien, "Id", "Id", hoaDonBan.NhanVienId);
            return View(hoaDonBan);
        }

        // GET: Admin/HoaDonBans/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hoaDonBan = await _context.HoaDonBan.FindAsync(id);
            if (hoaDonBan == null)
            {
                return NotFound();
            }
            ViewData["KhachHangId"] = new SelectList(_context.KhachHang, "Id", "Id", hoaDonBan.KhachHangId);
            ViewData["NhanVienId"] = new SelectList(_context.NhanVien, "Id", "Id", hoaDonBan.NhanVienId);
            return View(hoaDonBan);
        }

        // POST: Admin/HoaDonBans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NgayBan,KhachHangId,NhanVienId,DiaChiNhanHang,TongTien,PhuongThucThanhToan,TrangThai")] HoaDonBan hoaDonBan)
        {
            if (id != hoaDonBan.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hoaDonBan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HoaDonBanExists(hoaDonBan.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["KhachHangId"] = new SelectList(_context.KhachHang, "Id", "Id", hoaDonBan.KhachHangId);
            ViewData["NhanVienId"] = new SelectList(_context.NhanVien, "Id", "Id", hoaDonBan.NhanVienId);
            return View(hoaDonBan);
        }

        // GET: Admin/HoaDonBans/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hoaDonBan = await _context.HoaDonBan
                .Include(h => h.KhachHang)
                .Include(h => h.NhanVien)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (hoaDonBan == null)
            {
                return NotFound();
            }

            return View(hoaDonBan);
        }

        // POST: Admin/HoaDonBans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var hoaDonBan = await _context.HoaDonBan.FindAsync(id);
            if (hoaDonBan != null)
            {
                _context.HoaDonBan.Remove(hoaDonBan);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HoaDonBanExists(int id)
        {
            return _context.HoaDonBan.Any(e => e.Id == id);
        }
    }
}
