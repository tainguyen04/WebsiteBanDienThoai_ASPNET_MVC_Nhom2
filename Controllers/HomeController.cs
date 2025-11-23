using Microsoft.AspNetCore.Mvc;
using QLCHBanDienThoaiMoi.Helpers;
using QLCHBanDienThoaiMoi.Models;
using QLCHBanDienThoaiMoi.Services.Interfaces;

namespace QLCHBanDienThoaiMoi.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISanPhamService _sanPhamService;
        private readonly IGioHangService _gioHangService;
        private readonly SessionHelper _sessionHelper;
        private readonly IDanhMucSanPhamService _danhMucService;

        public HomeController(
            ISanPhamService sanPhamService,
            IGioHangService gioHangService,
            SessionHelper sessionHelper,
            IDanhMucSanPhamService danhMucService)
        {
            _sanPhamService = sanPhamService;
            _gioHangService = gioHangService;
            _sessionHelper = sessionHelper;
            _danhMucService = danhMucService;
        }

        // GET: Home
        public async Task<IActionResult> Index()
        {
            // Load danh mục cho Navbar
            ViewBag.DanhMuc = await _danhMucService.GetAllAsync();

            // Load sản phẩm trang chủ
            var sanPhams = await _sanPhamService.GetSanPhamHomePageAsync();

            // Xử lý giỏ hàng theo session
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
                return NotFound();

            return View(sanPham);
        }
    }
}
