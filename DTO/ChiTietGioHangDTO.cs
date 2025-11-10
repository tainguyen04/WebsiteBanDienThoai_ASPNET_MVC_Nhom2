namespace QLCHBanDienThoaiMoi.DTO
{
    public class ChiTietGioHangDTO
    {
        public string? SessionId { get; set; }
        public int? KhachHangId { get; set; }
        public int SanPhamId { get; set; }
        public string? HinhAnh { get; set; }
        public string? TenSanPham { get; set; }
        public int SoLuong { get; set; }
        public decimal GiaBan { get; set; }
        public decimal GiaKhuyenMai { get; set; }
        public decimal TongTien => GiaKhuyenMai * SoLuong;

    }
}
