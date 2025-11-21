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
using QLCHBanDienThoaiMoi.Services.Interfaces;
using SlugGenerator;

namespace QLCHBanDienThoaiMoi.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SanPhamsController : Controller
    {
        private readonly ISanPhamService _sanPhamService;

        public SanPhamsController(ISanPhamService sanPhamService)
        {
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
                if (result)
                {
                    TempData["SuccessMessage"] = "Thêm sản phẩm thành công!";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", "Có lỗi xảy ra khi thêm sản phẩm.");
                }      
                
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
        public async Task<IActionResult> Edit(IFormFile? file,int id, [Bind("Id,TenSanPham,GiaNhap,GiaBan,SoLuongTon,DanhMucId,HangSanXuat,MoTa,HinhAnh,KhuyenMaiId")] SanPham sanPham)
        {
            if (id != sanPham.Id)
            {
                return NotFound();
            }

            // Loại bỏ kiểm tra lỗi cho HinhAnh vì nó được xử lý riêng
            ModelState.Remove("HinhAnh");
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _sanPhamService.UpdateSanPhamAsync(file, sanPham);
                    if (!result)
                    {
                        return NotFound();
                    }
                    else
                    {
                        TempData["SuccessMessage"] = "Cập nhật sản phẩm thành công!";
                        return RedirectToAction(nameof(Index));
                    }
                        
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();
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
            var sp = await _sanPhamService.DeleteByIdAsync(id);
            if (sp == null)
            {
                return NotFound();
            }
            return RedirectToAction(nameof(Index));
        }
        // Load dữ liệu cho dropdown
        public async Task LoadDropDownData(SanPham? sanPham = null)
        {
            var khuyenMais = (await _sanPhamService.GetAllKhuyenMaiAsync())
                              .Select(k => new { k.Id, Ten = $"{k.TenKhuyenMai} - {k.GiaTri}%" });
            ViewData["DanhMucId"] = new SelectList(await _sanPhamService.GetAllDanhMucAsync(), "Id", "TenDanhMuc", sanPham?.DanhMucId);
            ViewData["KhuyenMaiId"] = new SelectList(khuyenMais, "Id", "Ten", sanPham?.KhuyenMaiId);
        }
    }
}
