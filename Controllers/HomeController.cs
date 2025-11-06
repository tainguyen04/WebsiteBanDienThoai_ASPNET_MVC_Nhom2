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
        private readonly ApplicationDbContext _context;
        private readonly SanPhamService _sanPhamService;
        public HomeController(ApplicationDbContext context,SanPhamService sanPhamService)
        {
            _context = context;
            _sanPhamService = sanPhamService;
        }

        // GET: Home
        public async Task<IActionResult> Index()
        {
            var sanPhams = await _sanPhamService.GetSanPhamsAsync();
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
