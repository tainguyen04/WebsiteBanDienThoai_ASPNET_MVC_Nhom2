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
    public class TaiKhoansController : Controller
    {
        private readonly ITaiKhoanService _taiKhoanService;

        public TaiKhoansController(ITaiKhoanService taiKhoanService)
        {
            _taiKhoanService = taiKhoanService;
        }

        // GET: Admin/TaiKhoans
        public async Task<IActionResult> Index()
        {
            return View(await _taiKhoanService.GetAllTaiKhoanAsync());
        }

        // GET: Admin/TaiKhoans/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taiKhoan = await _taiKhoanService.GetTaiKhoanByIdAsync(id);
            if (taiKhoan == null)
            {
                return NotFound();
            }

            return View(taiKhoan);
        }

        // GET: Admin/TaiKhoans/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/TaiKhoans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TenDangNhap,MatKhau,VaiTro,TrangThai")] TaiKhoan taiKhoan)
        {
            if (ModelState.IsValid)
            {
                var tk = await _taiKhoanService.CreateTaiKhoanAsync(taiKhoan);
                if (!tk)
                {
                    return NotFound();
                }
                else
                {
                    TempData["SuccessMessage"] = "Thêm tài khoản thành công";
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(taiKhoan);
        }

        // GET: Admin/TaiKhoans/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taiKhoan = await _taiKhoanService.GetTaiKhoanByIdAsync(id);
            if (taiKhoan == null)
            {
                return NotFound();
            }
            return View(taiKhoan);
        }

        // POST: Admin/TaiKhoans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id,VaiTro vaiTro)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var tk = await _taiKhoanService.UpdateTaiKhoanAsync(id.Value,vaiTro);
                    if (!tk)
                    {
                        return NotFound();
                    }
                    else
                    {
                        TempData["SuccessMessage"] = "Cập nhật vai trò tài khoản thành công";
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();
                }
                
            }
            var taiKhoan = await _taiKhoanService.GetTaiKhoanByIdAsync(id);
            if (taiKhoan == null) 
                return NotFound(); 
            return View(taiKhoan);
        }

        

        // POST: Admin/TaiKhoans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var taiKhoan = await _taiKhoanService.DeleteTaiKhoanAsync(id);
            if (!taiKhoan)
            {
                return NotFound();
            }
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LockTaiKhoan(int id)
        {
            var khoa = await _taiKhoanService.LockTaiKhoanAsync(id);
            if (!khoa)
            {
                return NotFound();
            }
            else
            {
                TempData["SuccessMessage"] = "Đã khóa tài khoản thành công";
                return RedirectToAction(nameof(Index));
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UnlockTaiKhoan(int id)
        {
            var khoa = await _taiKhoanService.UnlockTaiKhoanAsync(id);
            if (!khoa)
            {
                return NotFound();
            }
            else
            {
                TempData["SuccessMessage"] = "Đã mở khóa tài khoản thành công";
                return RedirectToAction(nameof(Index));
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassWord(int id)
        {
            var tk = await _taiKhoanService.ResetMatKhauAsync(id, "123456789");
            if (!tk)
            {
                return NotFound();
            }
            else
            {
                TempData["SuccessMessage"] = "Đã Reset mật khẩu về 123456789!";
                return RedirectToAction(nameof(Index));
            }
        }
       
    }
}
