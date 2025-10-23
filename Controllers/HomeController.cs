using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QLCHBanDienThoaiMoi.Data;
using QLCHBanDienThoaiMoi.Models;

namespace QLCHBanDienThoaiMoi.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Home
        public async Task<IActionResult> Index(int page = 1,int pageSize = 10)
        {
            var sanPhams = _context.SanPham
                            .Include(s => s.DanhMucSanPham)
                            .Include(s => s.KhuyenMai)
                            .AsNoTracking();
            var orderSanPham = sanPhams.OrderByDescending(s => s.KhuyenMai != null ? s.KhuyenMai.GiaTri : 0).ThenByDescending(s => s.Id);
            //Phân trang
            var pagedSanPhams = await orderSanPham.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            //Thông tin phân trang
            var totalItems = await sanPhams.CountAsync();
            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalItems = totalItems;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / pageSize);
           

            var soSanPham = await _context.GioHang.CountAsync();
            ViewBag.SoSanPham = soSanPham;
            return View(pagedSanPhams);
        }

        // GET: Home/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sanPham = await _context.SanPham
                .Include(s => s.DanhMucSanPham)
                .Include(s => s.KhuyenMai)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sanPham == null)
            {
                return NotFound();
            }

            return View(sanPham);
        }
    }

}
