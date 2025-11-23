using QLCHBanDienThoaiMoi.Models;
using System;

namespace QLCHBanDienThoaiMoi.Helpers
{
    public static class EnumExtensions
    {
        public static string GetBadgeHtml(TrangThaiHoaDon trangThai)
        {
            return trangThai switch
            {
                TrangThaiHoaDon.ChuaHoanThanh => "<span class='badge bg-warning text-dark'>Chưa hoàn thành</span>",
                TrangThaiHoaDon.HoanThanh => "<span class='badge bg-success'>Đã hoàn thành</span>",
                _ => "<span class='badge bg-secondary'>Không xác định</span>"
            };
        }
        public static string GetBadgeHtml(PhuongThucThanhToan phuongThuc)
        {
            return phuongThuc switch
            {
                PhuongThucThanhToan.TienMat => "<span class='badge bg-primary'>Tiền mặt</span>",
                PhuongThucThanhToan.ChuyenKhoan => "<span class='badge bg-info text-dark'>Chuyển khoản</span>",
                _ => "<span class='badge bg-secondary'>Không xác định</span>"
            };
        }
        public static string GetBadgeHtml(VaiTro vaiTro)
        {
            return vaiTro switch
            {
                VaiTro.Admin => "<span><i class='fas fa-crown me-1'></i> Quản trị</span>",
                VaiTro.User => "<span><i class='fas fa-user me-1'></i> Người dùng</span>",
                _ => "<span class='badge bg-secondary'>Không xác định</span>"
            };
        }

        public static string GetBadgeHtml(TrangThaiTaiKhoan trangThai)
        {
            return trangThai switch
            {
                TrangThaiTaiKhoan.Locked => "<span><i class='fas fa-lock me-1'></i> Đã khóa</span>",
                TrangThaiTaiKhoan.Active => "<span><i class='fas fa-check-circle me-1'></i> Hoạt động</span>",
                _ => "<span class='badge bg-secondary'>Không xác định</span>"
            };
        }

    }
}
