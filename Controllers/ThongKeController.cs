using Microsoft.AspNetCore.Mvc;
using QLCHBanDienThoaiMoi.Data;
using QLCHBanDienThoaiMoi.Models;

namespace QLCHBanDienThoaiMoi.Controllers
{
    public class ThongKeController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        public IActionResult Index()
        {
            ViewBag.DoanhThuHomNay = TinhDoanhThuTheoNgay(DateTime.Now);
            ViewBag.DoanhThuThangNay = TinhDoanhThuTheoThang(DateTime.Now.Month, DateTime.Now.Year);
            ViewBag.DoanhThuNamNay = TinhDoanhThuTheoNam(DateTime.Now.Year);
            return View();
        }

        private decimal TinhDoanhThuTheoNgay(DateTime ngay)
        {
            var doanhThu = db.HoaDonBan
                .Where(h => h.NgayBan.Date == ngay.Date && h.TrangThai == TrangThaiHoaDon.HoanThanh)
                .Select(h => h.ChiTietHoaDonBans.Sum(ct => ct.GiaBan * (1 - (ct.SanPham.KhuyenMai.GiaTri / 100m)) * ct.SoLuong))
                .Sum();

            return doanhThu;
        }

        private decimal TinhDoanhThuTheoThang(int thang, int nam)
        {
            var doanhThu = db.HoaDonBan
                .Where(h => h.NgayBan.Month == thang && h.NgayBan.Year == nam && h.TrangThai == TrangThaiHoaDon.HoanThanh)
                .Select(h => h.ChiTietHoaDonBans.Sum(ct => ct.GiaBan * (1 - (ct.SanPham.KhuyenMai.GiaTri / 100m)) * ct.SoLuong))
                .Sum();

            return doanhThu;
        }

        private decimal TinhDoanhThuTheoNam(int nam)
        {
            var doanhThu = db.HoaDonBan
                .Where(h => h.NgayBan.Year == nam && h.TrangThai == TrangThaiHoaDon.HoanThanh)
                .Select(h => h.ChiTietHoaDonBans.Sum(ct => ct.GiaBan * (1 - (ct.SanPham.KhuyenMai.GiaTri / 100m)) * ct.SoLuong))
                .Sum();

            return doanhThu;
        }

    }
}
