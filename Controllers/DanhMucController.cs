using Microsoft.AspNetCore.Mvc;
using QLCHBanDienThoaiMoi.Services;
using QLCHBanDienThoaiMoi.Services.Interfaces;
namespace QLCHBanDienThoaiMoi.Controllers
{
    public class DanhMucController : Controller
    {
        private readonly IDanhMucSanPhamService _danhMucService;

        public DanhMucController(IDanhMucSanPhamService danhMucService)
        {
            _danhMucService = danhMucService;
        }

        // --------------------------
        // VIEW SEARCH
        // --------------------------
        public async Task<IActionResult> Search(string? keyword)
        {
            var result = await _danhMucService.SearchAsync(keyword ?? "");
            ViewBag.Keyword = keyword;
            return View(result);
        }

        // --------------------------
        // API SEARCH
        // --------------------------
        [HttpGet]
        [Route("api/danhmuc/search")]
        public async Task<IActionResult> SearchApi([FromQuery] string? keyword)
        {
            var result = await _danhMucService.SearchAsync(keyword ?? "");
            return Ok(result);
        }
    }
}
