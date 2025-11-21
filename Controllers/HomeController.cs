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
    public class HomeController : Controller
    {
        private readonly SanPhamService _sanPhamService;
        private readonly GioHangService _gioHangService;
        private readonly SessionHelper _sessionHelper;
        public HomeController(SanPhamService sanPhamService, GioHangService gioHangService,SessionHelper sessionHelper)
        {
            _sanPhamService = sanPhamService;
            _gioHangService = gioHangService;
            _sessionHelper = sessionHelper;
        }
        // GET: Home
        public async Task<IActionResult> Index()
        {
            var sanPhams = await _sanPhamService.GetSanPhamHomePageAsync();
            string sessionId = _sessionHelper.EnsureSessionIdExists();
            int? khachHangId = _sessionHelper.GetKhachHangId();
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
