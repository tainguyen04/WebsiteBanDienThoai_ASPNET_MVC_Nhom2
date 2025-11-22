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

        // --------------------------
        // API SEARCH (JSON)
        // --------------------------
        [HttpGet]
        [Route("api/sanpham/search")]
        public async Task<IActionResult> SearchApi([FromQuery] string? keyword)
        {
            var result = await _sanPhamService.SearchAsync(keyword ?? "");
            return Ok(result);
        }
    }
}
