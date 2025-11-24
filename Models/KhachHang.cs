namespace QLCHBanDienThoaiMoi.Models
{
    public class KhachHang
    {
        public int Id { get; set; }
        public string? TenKhachHang { get; set; }
        public string? DiaChi { get; set; }
        public string? SoDienThoai { get; set; }
        public string? Email { get; set; }
        public TaiKhoan? TaiKhoan { get; set; }
        public ICollection<GioHang> GioHangs { get; set; } = new List<GioHang>();
        public ICollection<HoaDonBan> HoaDonBans { get; set; } = new List<HoaDonBan>();
    }
}