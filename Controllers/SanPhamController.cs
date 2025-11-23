using Microsoft.AspNetCore.Mvc;
using QLCHBanDienThoaiMoi.Services;
using QLCHBanDienThoaiMoi.Services.Interfaces;

namespace QLCHBanDienThoaiMoi.Controllers
{
    public class SanPhamController : Controller
    {
        private readonly ISanPhamService _sanPhamService;

        public SanPhamController(ISanPhamService sanPhamService)
        {
            _sanPhamService = sanPhamService;
        }

        // --------------------------
        // VIEW SEARCH
        // --------------------------
        public async Task<IActionResult> Search(string? keyword)
        {
            var result = await _sanPhamService.SearchAsync(keyword ?? "");
            ViewBag.Keyword = keyword;
            return View(result);

        }

        public async Task<IActionResult> TheoDanhMuc(int id)
        {
            var result = await _sanPhamService.GetByDanhMucAsync(id);

            if (result == null)
                return NotFound();

            ViewBag.IdDanhMuc = id;
            return View(result);
        }

    }
}
