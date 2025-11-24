
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QLCHBanDienThoaiMoi.Data;
using QLCHBanDienThoaiMoi.Helpers;
using QLCHBanDienThoaiMoi.Models;
using QLCHBanDienThoaiMoi.Services;
using QLCHBanDienThoaiMoi.Services.Interfaces;

namespace QLCHBanDienThoaiMoi.Controllers
{
    public class HoaDonBansController : Controller
    {
        private readonly IHoaDonBanService _hoaDonBanService;
        private readonly IGioHangService _gioHangService;
        private readonly SessionHelper _sessionHelper;
        private readonly IPhieuBaoHanhService _phieuBaoHanhService;
        private readonly IKhachHangService _khachHangService;
        public HoaDonBansController(IHoaDonBanService hoaDonBanService, IGioHangService gioHangService, SessionHelper sessionHelper, IPhieuBaoHanhService phieuBaoHanhService,IKhachHangService khachHangService)
        {
            _hoaDonBanService = hoaDonBanService;
            _gioHangService = gioHangService;
            _sessionHelper = sessionHelper;
            _phieuBaoHanhService = phieuBaoHanhService;
            _khachHangService = khachHangService;
        }
        public async Task<IActionResult> Index()
        {
            var userIdClaim = User.FindFirst("KhachHangId")?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                return RedirectToAction("Login", "Account");
            return View(await _hoaDonBanService.GetHoaDonBanByUserAsync(userId));
        }
        public async Task<IActionResult> Details(int id)
        {
            var userIdClaim = User.FindFirst("KhachHangId")?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                return RedirectToAction("Index", "Account");
            var hd = await _hoaDonBanService.GetHoaDonBanAsync(id);
            if(hd == null) return NotFound();
            if (hd.KhachHangId != userId) 
                return RedirectToAction("Index", "HoaDonBans");

            return View(hd);
        }
        public async Task<IActionResult> Edit(int id)
        {
            var userIdClaim = User.FindFirst("KhachHangId")?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                return RedirectToAction("Login", "Account");
            var hd = await _hoaDonBanService.GetHoaDonBanAsync(id); 
            if(hd == null) return NotFound();
            if (hd.KhachHangId != userId)
                return RedirectToAction("Index", "HoaDonBans");
            return View(hd);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,string diaChiNhanHang)
        {
            var userIdClaim = User.FindFirst("KhachHangId")?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                return RedirectToAction("Login", "Account");
            if (string.IsNullOrWhiteSpace(diaChiNhanHang))
            {
                TempData["ErrorMessage"] = "Địa chỉ không được để trống";
                return View();
            }
            var hoaDon = await _hoaDonBanService.UpdateDiaChiNhanHangAsync(id, userId, diaChiNhanHang);
                if (!hoaDon)
                {
                    return NotFound();
                }
                else
                {
                    TempData["SuccessMessage"] = "Cập nhật địa chỉ giao hàng thành công";
                    return RedirectToAction(nameof(Index));
                }
        }
        [HttpGet]
        public IActionResult Success(int id)
        {
            ViewBag.HoaDonBanId = id;
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Create(string selectedCartItems)
        {
            var userId = _sessionHelper.GetUserIdFromClaim();
            if (userId == null) return RedirectToAction("Login", "Account");

            if (string.IsNullOrEmpty(selectedCartItems))
                return RedirectToAction("Index", "GioHangs");

            var cartItemIds = selectedCartItems.Split(',')
              .Select(idStr => int.TryParse(idStr, out var id) ? (int?)id : null)
              .Where(id => id.HasValue)
              .ToList();

            if (!cartItemIds.Any())
                return RedirectToAction("Index", "GioHangs");

            // Lấy toàn bộ chi tiết giỏ hàng
            var cartItems = await _gioHangService.GetGioHangAsync(null, userId);
            

            var selectedItems = cartItems
                .Where(ci => cartItemIds.Contains(ci.SanPhamId) && ci.KhachHangId == userId)
                .ToList();

            if (!selectedItems.Any())
                //return RedirectToAction("Index", "GioHangs");
                return BadRequest();

            var khachHang = await _khachHangService.GetKhachHangById(userId.Value);

            var hoaDonBan = new HoaDonBan
            {
                KhachHangId = userId.Value,
                PhuongThucThanhToan = PhuongThucThanhToan.TienMat,
                DiaChiNhanHang = khachHang?.DiaChi ?? ""
            };
            ViewBag.KhachHang = khachHang;  
            ViewBag.SelectedItems = selectedItems;
            ViewBag.TotalAmount = selectedItems.Sum(ci => ci.SoLuong * ci.GiaKhuyenMai);
            ViewBag.SelectedCartItems = selectedCartItems;

            return View(hoaDonBan);
        }



        // POST: HoaDonBans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(HoaDonBan hoaDonBan, string selectedCartItems)
        {
            var userId = _sessionHelper.GetUserIdFromClaim();
            if (userId == null) return RedirectToAction("Login", "Account");

            if (string.IsNullOrEmpty(selectedCartItems))
            {
                TempData["ErrorMessage"] = "Không có sản phẩm được chọn";
                return RedirectToAction("Index", "GioHangs");
            }

            try
            {
                var cartItemIds = selectedCartItems.Split(',')
                    .Where(x => int.TryParse(x, out _))
                    .Select(int.Parse)
                    .ToList();

                // Lấy toàn bộ chi tiết giỏ hàng
                var gioHang = await _gioHangService.GetGioHangAsync(null, userId);

                if (gioHang == null || !gioHang.Any())
                {
                    TempData["ErrorMessage"] = "Giỏ hàng trống";
                    return RedirectToAction("Index", "GioHangs");
                }

                // Lọc theo Id của ChiTietGioHang
                var selectedItems = gioHang
                    .Where(ci => cartItemIds.Contains(ci.SanPhamId))
                    .ToList();

                if (!selectedItems.Any())
                {
                    TempData["ErrorMessage"] = "Không tìm thấy sản phẩm được chọn";
                    return RedirectToAction("Index", "GioHangs");
                }

                // Gán thông tin hóa đơn
                hoaDonBan.KhachHangId = userId.Value;
                hoaDonBan.NgayBan = DateTime.Now;
                hoaDonBan.TrangThai = TrangThaiHoaDon.ChuaHoanThanh;
                hoaDonBan.TongTien = selectedItems.Sum(ci => (int)(ci.SoLuong * ci.GiaKhuyenMai));

                // Tạo chi tiết hóa đơn từ chi tiết giỏ hàng
                hoaDonBan.ChiTietHoaDonBans = selectedItems.Select(ci => new ChiTietHoaDonBan
                {
                    SanPhamId = ci.SanPhamId,
                    SoLuong = ci.SoLuong,
                    GiaBan = (int)ci.GiaKhuyenMai
                }).ToList();

                // Lưu hóa đơn
                var result = await _hoaDonBanService.CreateHoaDonBanAsync(hoaDonBan, hoaDonBan.ChiTietHoaDonBans.ToList());

                if (result)
                {
                    await _phieuBaoHanhService.CreateAsync(hoaDonBan.Id);

                    // Xóa chi tiết giỏ hàng đã mua
                    foreach (var item in selectedItems)
                    {
                        await _gioHangService.DeletedSanPhamFromGioHangAsync(null, userId, item.SanPhamId);
                    }

                    return RedirectToAction("Success", new { id = hoaDonBan.Id });
                }

                TempData["ErrorMessage"] = "Tạo hóa đơn thất bại";
                return RedirectToAction("Create", new { selectedCartItems });
            }
            catch
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi tạo hóa đơn";
                return RedirectToAction("Create", new { selectedCartItems });
            }
        }


    }
}
