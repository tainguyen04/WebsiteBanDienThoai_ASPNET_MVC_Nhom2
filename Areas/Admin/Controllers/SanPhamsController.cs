using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using QLCHBanDienThoaiMoi.Data;
using QLCHBanDienThoaiMoi.Models;
using QLCHBanDienThoaiMoi.Services;
using SlugGenerator;

namespace QLCHBanDienThoaiMoi.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SanPhamsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly SanPhamService _sanPhamService;

        public SanPhamsController(ApplicationDbContext context, SanPhamService sanPhamService)
        {
            _context = context;
            _sanPhamService = sanPhamService;
        }

        // GET: SanPhams
        public async Task<IActionResult> Index()
        {
           
            return View(await _sanPhamService.GetSanPhamsAsync());
        }

        // GET: SanPhams/Details/5
        public async Task<IActionResult> Details(int? id)
        {
           var sanPham = await _sanPhamService.GetSanPhamByIdAsync(id);
            if (sanPham == null)
            {
                return NotFound();
            }

            return View(sanPham);
        }
        
        // GET: SanPhams/Create
        public async Task<IActionResult> Create()
        {
            await LoadDropDownData();
            return View();
        }

        // POST: SanPhams/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormFile file,[Bind("Id,TenSanPham,GiaNhap,GiaBan,SoLuongTon,DanhMucId,HangSanXuat,MoTa,HinhAnh,KhuyenMaiId")] SanPham sanPham)
        {
            
            if (ModelState.IsValid)
            {
                var result = await _sanPhamService.CreateSanPhamAsync(file,sanPham);
                if(result)
                    return RedirectToAction(nameof(Index));
                
            }
            await LoadDropDownData(sanPham);
            return View(sanPham);
        }

        // GET: SanPhams/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var sanPham = await _sanPhamService.GetSanPhamByIdAsync(id);
            if (sanPham == null)
            {
                return NotFound();
            }
            await LoadDropDownData(sanPham);

            return View(sanPham);
        }

        // POST: SanPhams/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(IFormFile file,int id, [Bind("Id,TenSanPham,GiaNhap,GiaBan,SoLuongTon,DanhMucId,HangSanXuat,MoTa,HinhAnh,KhuyenMaiId")] SanPham sanPham)
        {
            if (id != sanPham.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _sanPhamService.UpdateSanPhamAsync(file, sanPham);
                    if (!result)
                    {
                        return NotFound();
                    }
                    RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SanPhamExists(sanPham.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            await LoadDropDownData(sanPham);
            return View(sanPham);
        }

        // GET: SanPhams/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            return View(await _sanPhamService.DeleteByIdAsync(id));
        }

        // POST: SanPhams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sanPham = await _context.SanPham.FindAsync(id);
            if (sanPham != null)
            {
                if(!string.IsNullOrEmpty(sanPham.HinhAnh) && sanPham.HinhAnh != "default.png")
                {
                    var oldPath = Path.Combine("wwwroot", "images", sanPham.HinhAnh);
                    if(System.IO.File.Exists(oldPath))
                        System.IO.File.Delete(oldPath);
                }
                _context.SanPham.Remove(sanPham);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SanPhamExists(int id)
        {
            return _context.SanPham.Any(e => e.Id == id);
        }
        public async Task LoadDropDownData(SanPham? sanPham = null)
        {
            var khuyenMais = (await _sanPhamService.GetAllKhuyenMaiAsync())
                              .Select(k => new { k.Id, Ten = $"{k.TenKhuyenMai} - {k.GiaTri}%" });
            ViewData["DanhMucId"] = new SelectList(await _sanPhamService.GetAllDanhMucAsync(), "Id", "TenDanhMuc", sanPham?.DanhMucId);
            ViewData["KhuyenMaiId"] = new SelectList(khuyenMais, "Id", "Ten", sanPham?.KhuyenMaiId);
        }
    }
}
