using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLCHBanDienThoaiMoi.Models
{
    public class HoaDonBan
    {
        public int Id { get; set; }
        [Required]
        public DateTime NgayBan { get; set; }
        public int? KhachHangId { get; set; }
        public KhachHang? KhachHang { get; set; } 
        public int? NhanVienId { get; set; }
        public NhanVien? NhanVien { get; set; } 
        public PhuongThucThanhToan PhuongThucThanhToan { get; set; }
        public TrangThaiHoaDon TrangThai { get; set; }
        public ICollection<ChiTietHoaDonBan> ChiTietHoaDonBans { get; set; } = new List<ChiTietHoaDonBan>();
        [NotMapped]
        public int ThanhTien => (int)ChiTietHoaDonBans.Sum(ct => ct.GiaBan * (1 - (ct.SanPham?.KhuyenMai?.GiaTri ?? 0) / 100) * ct.SoLuong);
    }
    public enum PhuongThucThanhToan
    {
        TienMat,ChuyenKhoan
    }
    public enum TrangThaiHoaDon
    {
        HoanThanh, ChuaHoanThanh
    }
}
