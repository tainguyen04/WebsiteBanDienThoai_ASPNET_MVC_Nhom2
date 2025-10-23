using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QLCHBanDienThoaiMoi.Data;
using QLCHBanDienThoaiMoi.Models;

namespace QLCHBanDienThoaiMoi.Controllers
{
    public class GioHangsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GioHangsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: GioHangs
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.GioHang.Include(g => g.KhachHang).Include(g => g.SanPham).ThenInclude(g => g.KhuyenMai);
            
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: GioHangs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gioHang = await _context.GioHang
                .Include(g => g.KhachHang)
                .Include(g => g.SanPham)
                .FirstOrDefaultAsync(m => m.KhachHangId == id);
            if (gioHang == null)
            {
                return NotFound();
            }

            return View(gioHang);
        }

        
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gioHang = await _context.GioHang.FindAsync(id);
            if (gioHang == null)
            {
                return NotFound();
            }
            ViewData["KhachHangId"] = new SelectList(_context.KhachHang, "Id", "Id", gioHang.KhachHangId);
            ViewData["SanPhamId"] = new SelectList(_context.SanPham, "Id", "Id", gioHang.SanPhamId);
            return View(gioHang);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        // Thêm vào giỏ hàng khi người dùng chọn sản phẩm
        public async Task<IActionResult> AddToCart(int sanPhamId, int soLuong)
        {
            string sessionId = HttpContext.Session.GetString("CartSessionId")?.Trim();
            if(string.IsNullOrEmpty(sessionId))
            {
                // Nếu chưa có session, tạo mới
                sessionId = Guid.NewGuid().ToString();
                HttpContext.Session.SetString("CartSessionId", sessionId);
            }
            var existingCartItem = await _context.GioHang
                .AsNoTracking()
                .FirstOrDefaultAsync(g => g.SessionId == sessionId && g.SanPhamId == sanPhamId);
            if (existingCartItem != null)
            {
                // Nếu sản phẩm đã có trong giỏ hàng, cập nhật số lượng
                existingCartItem.SoLuong += soLuong;
                _context.Update(existingCartItem);
            }
            else
            {
                // Nếu sản phẩm chưa có trong giỏ hàng, thêm mới
                var newCartItem = new GioHang
                {
                    SessionId = sessionId,
                    KhachHangId = null,
                    SanPhamId = sanPhamId,
                    SoLuong = soLuong
                };
                _context.Add(newCartItem);
            }
            await _context.SaveChangesAsync();
            TempData["ThongBao"] = "Đã thêm vào giỏ hàng";
            return Redirect(Request.Headers["Referer"].ToString());
        }
        // POST: GioHangs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("KhachHangId,SanPhamId,SoLuong")] GioHang gioHang)
        {
            if (id != gioHang.KhachHangId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gioHang);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GioHangExists(gioHang.KhachHangId))
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
            ViewData["KhachHangId"] = new SelectList(_context.KhachHang, "Id", "Id", gioHang.KhachHangId);
            ViewData["SanPhamId"] = new SelectList(_context.SanPham, "Id", "Id", gioHang.SanPhamId);
            return View(gioHang);
        }

        // GET: GioHangs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gioHang = await _context.GioHang
                .Include(g => g.KhachHang)
                .Include(g => g.SanPham)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gioHang == null)
            {
                return NotFound();
            }

            return View(gioHang);
        }

        // POST: GioHangs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gioHang = await _context.GioHang.FindAsync(id);
            if (gioHang != null)
            {
                _context.GioHang.Remove(gioHang);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GioHangExists(int? id)
        {
            return _context.GioHang.Any(e => e.KhachHangId == id);
        }
    }
}
