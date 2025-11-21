
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
    public class NhaCungCapsController : Controller
    {
        private readonly INhaCungCapService _nhaCungCapService;

        public NhaCungCapsController(INhaCungCapService nhaCungCapService)
        {
            _nhaCungCapService = nhaCungCapService;
        }

        // GET: Admin/NhaCungCaps
        public async Task<IActionResult> Index()
        {
            return View(await _nhaCungCapService.GetAllNhaCungCapAsync());
        }
        //GET: Admin/NhaCungCaps/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var nhaCungCap = await _nhaCungCapService.GetNhaCungCapById(id);
            if (nhaCungCap == null)
            {
                return NotFound();
            }
            return View(nhaCungCap);
        }

        // POST: Admin/NhaCungCaps/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TenNCC,DiaChi,SoDienThoai,Email")] NhaCungCap nhaCungCap)
        {
            if (ModelState.IsValid)
            {
                var result = await _nhaCungCapService.CreateNhaCungCapAsync(nhaCungCap);
                if (!result)
                {
                    return NotFound();
                }
                else
                {
                    TempData["SuccessMessage"] = "Thêm nhà cung cấp thành công!";
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(nhaCungCap);
        }

        // GET: Admin/NhaCungCaps/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var nhaCungCap = await _nhaCungCapService.GetNhaCungCapById(id);
            if (nhaCungCap == null)
            {
                return NotFound();
            }
            return View(nhaCungCap);
        }

        // POST: Admin/NhaCungCaps/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TenNCC,DiaChi,SoDienThoai,Email")] NhaCungCap nhaCungCap)
        {
            if (id != nhaCungCap.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var ncc = await _nhaCungCapService.UpdateNhaCungCapAsync(nhaCungCap);
                    if (!ncc)
                    {
                        return NotFound();
                    }
                    else
                    {
                        TempData["SuccessMessage"] = "Cập nhật nhà cung cấp thành công!";
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();
                }
            }
            return View(nhaCungCap);
        }


        // POST: Admin/NhaCungCaps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var nhaCungCap = await _nhaCungCapService.DeleteNhaCungCapAsync(id);
            if (!nhaCungCap)
            {
                return NotFound();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
