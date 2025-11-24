using QLCHBanDienThoaiMoi.Models;

namespace QLCHBanDienThoaiMoi.DTO
{
    public class SanPhamDTO
    {
        public int Id { get; set; }
        public int DanhMucId { get; set; }
        public string? TenSanPham { get; set; }
        public int GiaBan { get; set; }
        public int SoLuongTon { get; set; }
        public string? HangSanXuat { get; set; }
        public string? MoTa { get; set; }
        public string? HinhAnh { get; set; }
        public string? TenKhuyenMai { get; set; }
        public decimal GiaKhuyenMai { get; set; }
        public decimal? PhanTramKhuyenMai { get; set; }
        public KhuyenMai? KhuyenMai { get; set; }

    }
}
