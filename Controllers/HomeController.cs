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

namespace QLCHBanDienThoaiMoi.Controllers
{
    public class HomeController : Controller
    {
        private readonly SanPhamService _sanPhamService;
        private readonly GioHangService _gioHangService;
        public HomeController(SanPhamService sanPhamService, GioHangService gioHangService)
        {
            _sanPhamService = sanPhamService;
            _gioHangService = gioHangService;
        }
        public string EnsureSessionIdExists()
        {
            var sesionId = HttpContext.Session.GetString("CartSessionId")?.Trim();
            if (string.IsNullOrEmpty(sesionId))
            {
                sesionId = Guid.NewGuid().ToString();
                HttpContext.Session.SetString("CartSessionId", sesionId);
            }
            return sesionId;
        }
        public int? GetKhachHangId()
        {
            return HttpContext.Session.GetInt32("KhachHangId");
        }
        // GET: Home
        public async Task<IActionResult> Index()
        {
            var sanPhams = await _sanPhamService.GetSanPhamHomePageAsync();
            string sessionId = EnsureSessionIdExists();
            int? khachHangId = GetKhachHangId();
            await _gioHangService.CreateGioHangAsync(sessionId, khachHangId);
            return View(sanPhams);
        }

        // GET: Home/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var sanPham = await _sanPhamService.GetSanPhamByIdAsync(id);
            if (sanPham == null)
            {
                return NotFound();
            }

            return View(sanPham);
        }
    }

}
