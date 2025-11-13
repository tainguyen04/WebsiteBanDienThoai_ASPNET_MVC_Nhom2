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
    public class GioHangsController : Controller
    {
        private readonly GioHangService _gioHangService;
        private readonly SessionHelper _sessionHelper;
        public GioHangsController(GioHangService gioHangService,SessionHelper sessionHelper)
        {
            _gioHangService = gioHangService;
            _sessionHelper = sessionHelper;
        }
        // GET: GioHangs
        public async Task<IActionResult> Index()
        {
            var sessionId = _sessionHelper.EnsureSessionIdExists();
            int? khachHangId = _sessionHelper.GetKhachHangId(); // Lấy từ người dùng đăng nhập nếu có
            Console.WriteLine($"SessionId: {sessionId}, KhachHangId: {khachHangId}");
            var gioHang = await _gioHangService.GetGioHangAsync(sessionId, khachHangId);
            return View(gioHang);
        }
     
        [HttpPost]
        [ValidateAntiForgeryToken]
        // Thêm vào giỏ hàng khi người dùng chọn sản phẩm
        public async Task<IActionResult> AddToCart(int sanPhamId, int soLuong)
        {
            string sessionId = _sessionHelper.EnsureSessionIdExists();
            int? khachHangId = _sessionHelper.GetKhachHangId();
            await _gioHangService.AddToCardAsync(sessionId, sanPhamId, khachHangId, soLuong);
            TempData["ThongBao"] = "Đã thêm vào giỏ hàng";
            return Redirect(Request.Headers["Referer"].ToString());
        }
        

        

        // POST: GioHangs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int sanPhamId)
        {
            string? sessionId = _sessionHelper.EnsureSessionIdExists();
            int? khachHangId = _sessionHelper.GetKhachHangId();
            var gioHang = await _gioHangService.DeletedSanPhamFromGioHangAsync(sessionId,khachHangId,sanPhamId);
            return RedirectToAction(nameof(Index));
        }
    }
}
