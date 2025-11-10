using System.ComponentModel.DataAnnotations;

namespace QLCHBanDienThoaiMoi.Models
{
    public class PhieuBaoHanh
    {
        public int HoaDonBanId { get; set; }
        public int SanPhamId { get; set; }

        public DateTime NgayLap { get; set; } = DateTime.Now;
        public DateTime NgayHetHan { get; set; } = DateTime.Now.AddMonths(12);
        public string? MoTa { get; set; }
        public TrangThaiBaoHanh TrangThai { get; set; } = TrangThaiBaoHanh.DangBaoHanh;

        public ChiTietHoaDonBan ChiTietHoaDonBan { get; set; } = null!;
        public HoaDonBan HoaDonBan { get; set; } = null!;
        public SanPham SanPham { get; set; } = null!;

    }
    public enum TrangThaiBaoHanh
    {
        DangBaoHanh,
        HetHan
    }
}
