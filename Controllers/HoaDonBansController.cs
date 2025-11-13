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

namespace QLCHBanDienThoaiMoi.Controllers
{
    public class HoaDonBansController : Controller
    {
        private readonly HoaDonBanService _hoaDonBanService;
        private readonly GioHangService _gioHangService;
        private readonly SessionHelper _sessionHelper;
        public HoaDonBansController(HoaDonBanService hoaDonBanService, GioHangService gioHangService, SessionHelper sessionHelper)
        {
            _hoaDonBanService = hoaDonBanService;
            _gioHangService = gioHangService;
            _sessionHelper = sessionHelper;
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
            if (string.IsNullOrEmpty(selectedCartItems))
            {
                return RedirectToAction("Index", "GioHangs");
            }
            var cartItemIds = selectedCartItems.Split(',')
               .Select(idStr => int.TryParse(idStr, out var id) ? (int?)id : null)
               .Where(id => id.HasValue)
               .ToList();
            if (!cartItemIds.Any())
            {
                return RedirectToAction("Index", "GioHangs");
            }
            var sessionId = _sessionHelper.EnsureSessionIdExists();

            var cartItems = await _gioHangService.GetGioHangAsync(sessionId, null);
            var selectedItems = cartItems
                                .Where(ci => cartItemIds.Contains(ci.SanPhamId) && ci.SessionId == sessionId)
                                .ToList();

            var hoaDonBan = new HoaDonBan
            {
                NgayBan = DateTime.Now,
                PhuongThucThanhToan = PhuongThucThanhToan.TienMat,
                TrangThai = TrangThaiHoaDon.ChuaHoanThanh,
                DiaChiNhanHang = "",
                ChiTietHoaDonBans = selectedItems.Select(cartItems => new ChiTietHoaDonBan
                {
                    SanPhamId = cartItems.SanPhamId,
                    SoLuong = cartItems.SoLuong,
                    GiaBan = (int)cartItems.GiaKhuyenMai,
                }).ToList()
            };
            
            return View(hoaDonBan);
        }


        // POST: HoaDonBans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(HoaDonBan hoaDonBan,string selectedCartItems)
        {
            var sessionId = _sessionHelper.EnsureSessionIdExists();
            var gioHang = await _gioHangService.GetGioHangAsync(sessionId, null);
            if(gioHang == null)
            {
                return RedirectToAction("Index", "GioHangs", new { message = "Your cart is empty." });
            }
            var cartItemIds = selectedCartItems.Split(',')
               .Select(idStr => int.TryParse(idStr, out var id) ? (int?)id : null)
               .Where(id => id.HasValue)
               .ToList();
            if (!cartItemIds.Any())
            {
                return RedirectToAction("Index", "GioHang");
            }
            var selectedItems = gioHang
                                .Where(ci => cartItemIds.Contains(ci.SanPhamId) && ci.SessionId == sessionId)
                                .ToList();
            hoaDonBan.ChiTietHoaDonBans = selectedItems.Select(cartItems => new ChiTietHoaDonBan
            {
                SanPhamId = cartItems.SanPhamId,
                SoLuong = cartItems.SoLuong,
                GiaBan = (int)cartItems.GiaKhuyenMai,
            }).ToList();

            hoaDonBan.NgayBan = DateTime.Now;
            hoaDonBan.TrangThai = TrangThaiHoaDon.HoanThanh;
            hoaDonBan.TongTien = (int)hoaDonBan.ChiTietHoaDonBans.Sum(ct => ct.SoLuong * ct.GiaBan);

            var result = await _hoaDonBanService.CreateHoaDonBanAsync(hoaDonBan,hoaDonBan.ChiTietHoaDonBans.ToList());

            if (result)
            {
                foreach (var cartItem in hoaDonBan.ChiTietHoaDonBans.ToList())
                {
                    await _gioHangService.DeletedSanPhamFromGioHangAsync(sessionId, null, cartItem.SanPhamId);
                }
                return RedirectToAction("Success", new { id = hoaDonBan.Id });
            }
            else
            {
                return BadRequest("Failed to create sales invoice.");
            }
        }

    }
}
