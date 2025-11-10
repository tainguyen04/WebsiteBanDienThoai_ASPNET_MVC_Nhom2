using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLCHBanDienThoaiMoi.Models
{
    public class HoaDonBan
    {
        public int Id { get; set; }
        [Required]
        public DateTime NgayBan { get; set; } = DateTime.Now;
        public int? KhachHangId { get; set; }
        public KhachHang? KhachHang { get; set; } 
        public int? NhanVienId { get; set; }
        public NhanVien? NhanVien { get; set; } 
        public int TongTien { get; set; }
        public PhuongThucThanhToan PhuongThucThanhToan { get; set; }
        public TrangThaiHoaDon TrangThai { get; set; } = TrangThaiHoaDon.ChuaHoanThanh;
        public ICollection<ChiTietHoaDonBan> ChiTietHoaDonBans { get; set; } = new List<ChiTietHoaDonBan>();
    }
    public enum PhuongThucThanhToan
    {
        [Display(Name = "Tiền mặt")]
        TienMat,
        [Display(Name = "Chuyển khoản")]
        ChuyenKhoan
    }
    public enum TrangThaiHoaDon
    {
        HoanThanh, 
        ChuaHoanThanh
    }
}
