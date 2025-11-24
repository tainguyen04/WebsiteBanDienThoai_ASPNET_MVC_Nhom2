using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QLCHBanDienThoaiMoi.Data;
using QLCHBanDienThoaiMoi.Models;
using QLCHBanDienThoaiMoi.Services.Interfaces;

namespace QLCHBanDienThoaiMoi.Controllers
{
    public class KhachHangsController : Controller
    {
        private readonly IKhachHangService _khachHangService;

        public KhachHangsController(IKhachHangService khachHangService)
        {
            _khachHangService = khachHangService;
        }

        // GET: KhachHangs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var khachHang = await _khachHangService.GetKhachHangById(id.Value);
            if (khachHang == null)
            {
                return NotFound();
            }

            return View(khachHang);
        }
        // GET: KhachHangs/Edit/5
        public async Task<IActionResult> Edit()
        {
            var userIdClaim = User.FindFirst("KhachHangId")?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                return RedirectToAction("Index", "Home");

            var khachHang = await _khachHangService.GetKhachHangById(userId);
            if (khachHang == null)
            {
                return NotFound();
            }
            return View(khachHang);
        }

        // POST: KhachHangs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("Id,TenKhachHang,DiaChi,SoDienThoai,Email")] KhachHang khachHang)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userIdClaim = User.FindFirst("KhachHangId")?.Value;
                    if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                        return RedirectToAction("Index", "Home");
                    khachHang.Id = userId;
                    var kh = await _khachHangService.UpdateKhachHangAsync(khachHang);
                    if (!kh)
                    {
                        return BadRequest();
                    }
                    else
                    {
                        TempData["SuccessMessage"] = "Cập nhật thông tin thành công";
                        return RedirectToAction("Details", new { id = userId });
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();
                }
            }
            return View(khachHang);
        }
    }
}
