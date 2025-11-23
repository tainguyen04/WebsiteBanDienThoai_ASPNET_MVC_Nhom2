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
        public async Task<IActionResult> DanhMucPartial()
        {
            var danhMucs = await _danhMucService.GetAllAsync();
            return PartialView("_DanhMucPartial", danhMucs);
        }
    }
}
