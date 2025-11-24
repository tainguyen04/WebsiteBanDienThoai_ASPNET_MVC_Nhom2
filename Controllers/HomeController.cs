using Microsoft.AspNetCore.Mvc;
using QLCHBanDienThoaiMoi.Helpers;
using QLCHBanDienThoaiMoi.Models;
using QLCHBanDienThoaiMoi.Services.Interfaces;
using QLCHBanDienThoaiMoi.ViewModel;

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
        public async Task<IActionResult> Index(int? danhMucId)
        {
            // Load danh mục cho Navbar
            ViewBag.DanhMuc = await _danhMucService.GetAllAsync();

            // Load sản phẩm trang chủ
            var sanPhams = await _sanPhamService.GetSanPhamHomePageAsync();
            var dm = await _sanPhamService.GetAllDanhMucAsync();
            if(danhMucId.HasValue)
                sanPhams = sanPhams.Where(sp => sp.DanhMucId == danhMucId).ToList();
            var model = new HomeIndexViewModel
            {
                SanPham = sanPhams,
                DanhMucSanPham = dm,
            };

            // Xử lý giỏ hàng theo session
            string sessionId = _sessionHelper.EnsureSessionIdExists();
            int? khachHangId = _sessionHelper.GetKhachHangId();
            await _gioHangService.CreateGioHangAsync(sessionId, khachHangId);
            var gioHang = await _gioHangService.GetGioHangAsync(sessionId, khachHangId);
            int soSanPham = gioHang.Sum(ct => ct.SoLuong);
            ViewBag.soSanPham = soSanPham;
            return View(model);
        }
        public async Task<IActionResult> LoadMoreProducts(int skip = 0)
        {
            var moreProducts = await _sanPhamService.GetSanPhamSkipTakeAsync(skip, 10);
            if (!moreProducts.Any())
                return Content(""); // Không còn sản phẩm

            return PartialView("_SanPhamCardPartial", moreProducts);
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
