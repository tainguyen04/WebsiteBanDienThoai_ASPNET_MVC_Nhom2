using QLCHBanDienThoaiMoi.Models;

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
    }
}
