using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QLCHBanDienThoaiMoi.Data;
using QLCHBanDienThoaiMoi.Models;
using QLCHBanDienThoaiMoi.Services;

namespace QLCHBanDienThoaiMoi.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DanhMucSanPhamsController : Controller
    {
        private readonly DanhMucSanPhamService _danhMucSanPhamService;

        public DanhMucSanPhamsController(DanhMucSanPhamService danhMucSanPhamService)
        {
            _danhMucSanPhamService = danhMucSanPhamService;
        }

        // GET: DanhMucSanPhams
        public async Task<IActionResult> Index()
        {
            return View(await _danhMucSanPhamService.GetAllDanhMucSanPhamAsync());
        }
        //GERT: Admin/DanhMucSanPhams/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var danhMucSanPham = await _danhMucSanPhamService.GetDanhMucSanPhamByIdAsync(id);
            if (danhMucSanPham == null)
            {
                return NotFound();
            }

            return View(danhMucSanPham);
        }

        // GET: DanhMucSanPhams/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DanhMucSanPhams/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TenDanhMuc")] DanhMucSanPham danhMucSanPham)
        {
            if (ModelState.IsValid)
            {
                var dm = await _danhMucSanPhamService.AddDanhMucSanPhamAsync(danhMucSanPham);
                if (!dm)
                {
                    return NotFound();
                }
                else
                {
                    TempData["SuccessMessage"] = "Thêm danh mục sản phẩm thành công!";
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(danhMucSanPham);
        }

        // GET: DanhMucSanPhams/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var danhMucSanPham = await _danhMucSanPhamService.GetDanhMucSanPhamByIdAsync(id);
            if (danhMucSanPham == null)
            {
                return NotFound();
            }
            return View(danhMucSanPham);
        }

        // POST: DanhMucSanPhams/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TenDanhMuc")] DanhMucSanPham danhMucSanPham)
        {
            if (id != danhMucSanPham.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var dm = await _danhMucSanPhamService.UpdateDanhMucSanPhamAsync(danhMucSanPham);
                    if (!dm)
                    {
                    if (!DanhMucSanPhamExists(danhMucSanPham.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        TempData["SuccessMessage"] = "Cập nhật danh mục sản phẩm thành công!";
                        return RedirectToAction(nameof(Index));
                    }
            return View(danhMucSanPham);
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();
                }
                
            var danhMucSanPham = await _context.DanhMucSanPham
                .FirstOrDefaultAsync(m => m.Id == id);
            if (danhMucSanPham == null)
            {
                return NotFound();
            }

            return View(danhMucSanPham);
        }

        // POST: DanhMucSanPhams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dm = await _danhMucSanPhamService.DeleteDanhMucSanPhamAsync(id);
            if (!dm)
            {
                return NotFound();
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }

        
    }
}
