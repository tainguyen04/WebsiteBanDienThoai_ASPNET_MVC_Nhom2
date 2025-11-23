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

namespace QLCHBanDienThoaiMoi.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class KhachHangsController : Controller
    {
        private readonly IKhachHangService _khachHangService;

        public KhachHangsController(IKhachHangService khachHangService)
        {
            _khachHangService = khachHangService;
        }

        // GET: Admin/KhachHangs
        public async Task<IActionResult> Index()
        {
            return View(await _khachHangService.GetAllKhachHangAsync());
        }

        // GET: Admin/KhachHangs/Details/5
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

        

        // GET: Admin/KhachHangs/Edit/5
        public async Task<IActionResult> Edit(int? id)
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

        // POST: Admin/KhachHangs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TenKhachHang,DiaChi,SoDienThoai,Email")] KhachHang khachHang)
        {
            if (id != khachHang.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var kh = await _khachHangService.UpdateKhachHangAsync(khachHang);
                    if (!kh)
                    {
                        return NotFound();
                    }
                    else
                    {
                        TempData["SuccessMessage"] = "Cập nhật thông tin khách hàng thành công";
                        return RedirectToAction(nameof(Index));
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
