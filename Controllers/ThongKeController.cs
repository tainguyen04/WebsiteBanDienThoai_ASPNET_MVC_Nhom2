using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLCHBanDienThoaiMoi.Data;
using QLCHBanDienThoaiMoi.Models;
using System;
using System.Linq;

namespace QLCHBanDienThoaiMoi.Controllers
{
    public class ThongKeController : Controller
    {
        private readonly ApplicationDbContext db;

        public ThongKeController(ApplicationDbContext context)
        {
            db = context;
        }

        public IActionResult Index()
        {
            var today = DateTime.Now;

            ViewBag.DoanhThuHomNay = TinhDoanhThuTheoNgay(today);
            ViewBag.DoanhThuThangNay = TinhDoanhThuTheoThang(today.Month, today.Year);
            ViewBag.DoanhThuNamNay = TinhDoanhThuTheoNam(today.Year);

            return View();
        }

        private decimal TinhDoanhThuTheoNgay(DateTime ngay)
        {
            var hoaDons = db.HoaDonBan
                .Include(h => h.ChiTietHoaDonBans)
                    .ThenInclude(ct => ct.SanPham)
                .Where(h => h.NgayBan.Date == ngay.Date && h.TrangThai == TrangThaiHoaDon.HoanThanh)
                .ToList(); // dữ liệu về bộ nhớ

            decimal doanhThu = 0;

            foreach (var h in hoaDons)
            {
                foreach (var ct in h.ChiTietHoaDonBans)
                {
                    decimal khuyenMai = 0;
                    if (ct.SanPham != null)
                        khuyenMai = ct.SanPham.KhuyenMai; // trực tiếp, vì là decimal

                    doanhThu += ct.GiaBan * (1 - khuyenMai / 100m) * ct.SoLuong;
                }
            }

            return doanhThu;
        }


        private decimal TinhDoanhThuTheoThang(int thang, int nam)
        {
            var hoaDons = db.HoaDonBan
                .Include(h => h.ChiTietHoaDonBans)
                    .ThenInclude(ct => ct.SanPham)
                .Where(h => h.NgayBan.Month == thang && h.NgayBan.Year == nam && h.TrangThai == TrangThaiHoaDon.HoanThanh)
                .ToList();

            decimal doanhThu = 0;

            foreach (var h in hoaDons)
            {
                foreach (var ct in h.ChiTietHoaDonBans)
                {
                    decimal khuyenMai = 0;
                    if (ct.SanPham != null)
                        khuyenMai = ct.SanPham.KhuyenMai;

                    doanhThu += ct.GiaBan * (1 - khuyenMai / 100m) * ct.SoLuong;
                }
            }

            return doanhThu;
        }

        private decimal TinhDoanhThuTheoNam(int nam)
        {
            var hoaDons = db.HoaDonBan
                .Include(h => h.ChiTietHoaDonBans)
                    .ThenInclude(ct => ct.SanPham)
                .Where(h => h.NgayBan.Year == nam && h.TrangThai == TrangThaiHoaDon.HoanThanh)
                .ToList();

            decimal doanhThu = 0;

            foreach (var h in hoaDons)
            {
                foreach (var ct in h.ChiTietHoaDonBans)
                {
                    decimal khuyenMai = 0;
                    if (ct.SanPham != null)
                        khuyenMai = ct.SanPham.KhuyenMai;

                    doanhThu += ct.GiaBan * (1 - khuyenMai / 100m) * ct.SoLuong;
                }
            }

            return doanhThu;
        }

    }
}
