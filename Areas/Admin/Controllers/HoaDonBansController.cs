
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QLCHBanDienThoaiMoi.Data;
using QLCHBanDienThoaiMoi.DTO;
using QLCHBanDienThoaiMoi.Models;
using QLCHBanDienThoaiMoi.Services.Interfaces;

namespace QLCHBanDienThoaiMoi.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HoaDonBansController : Controller
    {
        private readonly IHoaDonBanService _hoaDonBanService;
        private readonly IKhachHangService _khachHangService;
        private readonly INhanVienService _nhanVienService;

        public HoaDonBansController(IHoaDonBanService hoaDonBanService, IKhachHangService khachHangService, INhanVienService nhanVienService)
        {
            _hoaDonBanService = hoaDonBanService;
            _khachHangService = khachHangService;
            _nhanVienService = nhanVienService;
        }

        // GET: Admin/HoaDonBans
        public async Task<IActionResult> Index()
        {
            return View(await _hoaDonBanService.GetAllHoaDonBanAsync());
        }

        // GET: Admin/HoaDonBans/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hoaDonBan = await _hoaDonBanService.GetHoaDonBanAsync(id.Value);
            if (hoaDonBan == null)
            {
                return NotFound();
            }
            return View(hoaDonBan);
        }

        // GET: Admin/HoaDonBans/Create
        public async Task<IActionResult> Create()
        {
            await LoadDataUser();
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

            }
            await LoadDataUser(hoaDonBan);
            return View(hoaDonBan);
        }

        // GET: Admin/HoaDonBans/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hoaDonBan = await _hoaDonBanService.GetHoaDonBanAsync(id.Value);
            if (hoaDonBan == null)
            {
                return NotFound();
            }
            var hoaDonBanDTO = new UpdateHoaDonBanDTO
            {
                Id = hoaDonBan.Id,
                TrangThai = hoaDonBan.TrangThai,
                DiaChiNhanHang = hoaDonBan.DiaChiNhanHang
            };
            await LoadDataUser(hoaDonBan);
            return View(hoaDonBanDTO);
        }

        // POST: Admin/HoaDonBans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateHoaDonBanDTO hoaDonBanDTO)
        {
            hoaDonBanDTO.Id = id;

            if (ModelState.IsValid)
            {
                try
                {
                   var hd = await _hoaDonBanService.UpdateHoaDonBanAsync(hoaDonBanDTO);
                    if (!hd)
                    {
                        return NotFound();
                    }
                    else
                    {
                        TempData["SuccessMessage"] = "Cập nhật trạng thái hóa đơn thành công!";
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();
                }
            }
            var hoaDonBan = await _hoaDonBanService.GetHoaDonBanAsync(id);
            if (hoaDonBan == null)
            {
                return NotFound();
            }
            await LoadDataUser(hoaDonBan);
            return View(hoaDonBanDTO);
        }

        
        // POST: Admin/HoaDonBans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var hoaDonBan = await _hoaDonBanService.DeleteHoaDonBanAsync(id);
            if (!hoaDonBan)
            {
                return NotFound();
            }
            return RedirectToAction(nameof(Index));
        }
        public async Task LoadDataUser(HoaDonBan? hoaDonBan = null)
        {
            var khList = await _khachHangService.GetAllKhachHangAsync();
            var nvList = await _nhanVienService.GetAllNhanVienAsync();
            ViewData["KhachHangId"] = new SelectList(khList, "Id", "TenKhachHang",hoaDonBan?.KhachHangId);
            ViewData["NhanVienId"] = new SelectList(nvList, "Id", "TenNhanVien",hoaDonBan?.NhanVienId);
        }
    }
}
