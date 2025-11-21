using QLCHBanDienThoaiMoi.Models;

namespace QLCHBanDienThoaiMoi.DTO
{
    public class UpdateHoaDonBanDTO
    {
        public int Id { get; set; }
        public TrangThaiHoaDon TrangThai { get; set; }
        public string DiaChiNhanHang { get; set; } = string.Empty;
    }
}
