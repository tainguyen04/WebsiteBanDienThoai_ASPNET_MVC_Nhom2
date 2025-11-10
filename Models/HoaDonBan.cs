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

        public ICollection<ChiTietHoaDonBan> ChiTietHoaDonBans { get; set; } = new List<ChiTietHoaDonBan>();

        [NotMapped]
        public decimal ThanhTien => ChiTietHoaDonBans.Sum(ct => ct.GiaBan * (1 - ct.SanPham.KhuyenMai / 100) * ct.SoLuong);

        public TrangThaiHoaDon TrangThai { get; set; }  // ✅ Đã sửa kiểu dữ liệu
    }

    public enum PhuongThucThanhToan
    {
        TienMat,
        ChuyenKhoan
    }

    public enum TrangThaiHoaDon
    {
        ChoXacNhan,
        DangGiao,
        HoanThanh,
        DaHuy
    }
}
