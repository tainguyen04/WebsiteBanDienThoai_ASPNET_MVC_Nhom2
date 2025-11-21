
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
using QLCHBanDienThoaiMoi.Services.Interfaces;

namespace QLCHBanDienThoaiMoi.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class KhuyenMaisController : Controller
    {
        private readonly IKhuyenMaiService _khuyenMaiService;

        public KhuyenMaisController(IKhuyenMaiService khuyenMaiService)
        {
            _khuyenMaiService = khuyenMaiService;
        }

        // GET: Admin/KhuyenMais
        public async Task<IActionResult> Index()
        {
            return View(await _khuyenMaiService.GetAllKhuyenMaiAsync());
        }
        //GET: Admin/KhuyenMais/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var khuyenMai = await _khuyenMaiService.GetKhuyenMaiById(id);
            if (khuyenMai == null)
            {
                return NotFound();
            }
            return View(khuyenMai);
        }
        // POST: Admin/KhuyenMais/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TenKhuyenMai,MoTa,LoaiKhuyenMai,NgayBatDau,NgayKetThuc,GiaTri")] KhuyenMai khuyenMai)
        {
            if (ModelState.IsValid)
            {
                var result = await _khuyenMaiService.CreateKhuyenMaiAsync(khuyenMai);
                if (result)
                {
                    TempData["SuccessMessage"] = "Thêm khuyến mãi thành công!";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", "Có lỗi xảy ra khi thêm khuyến mãi.");
                }

            }
            return View(khuyenMai);
        }

        // GET: Admin/KhuyenMais/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var khuyenMai = await _khuyenMaiService.GetKhuyenMaiById(id);
            if (khuyenMai == null)
            {
                return NotFound();
            }
            return View(khuyenMai);
        }

        // POST: Admin/KhuyenMais/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TenKhuyenMai,MoTa,LoaiKhuyenMai,NgayBatDau,NgayKetThuc,GiaTri")] KhuyenMai khuyenMai)
        {
            if (id != khuyenMai.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _khuyenMaiService.UpdateKhuyenMaiAsync(khuyenMai);
                    if (!result)
                    {
                        ModelState.AddModelError("", "Có lỗi xảy ra khi cập nhật khuyến mãi.");
                    }
                    else
                    {
                        TempData["SuccessMessage"] = "Cập nhật khuyến mãi thành công!";
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();
                }
            }
            return View(khuyenMai);
        }

        // POST: Admin/KhuyenMais/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _khuyenMaiService.DeleteKhuyenMaiAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
