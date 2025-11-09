using System.ComponentModel.DataAnnotations.Schema;

namespace QLCHBanDienThoaiMoi.Models
{
    public class SanPham
    {
        
        public int Id { get;set; }
        public string TenSanPham { get; set; } = null!;
        public int GiaNhap { get; set; }
        public int GiaBan { get; set; }
        public int SoLuongTon { get; set; }
        public int DanhMucId { get; set; }
        public int? KhuyenMaiId { get; set; }
        


        public string HangSanXuat { get; set; } = null!;
        public string MoTa { get; set; } = null!;
        public string? HinhAnh { get; set; }
        [NotMapped]
        public int GiaKhuyenMai
        {
            get
            {
                // Logic tính toán: Nếu có khuyến mãi, giảm %; ngược lại trả giá gốc
                if (KhuyenMai == null || KhuyenMai.GiaTri <= 0)
                    return GiaBan;

                return (int)(GiaBan * (1 - (KhuyenMai.GiaTri / 100)));  // 100m để chính xác decimal
            }
        }

        public DanhMucSanPham? DanhMucSanPham { get; set; }
        public KhuyenMai? KhuyenMai { get; set; }
        public ICollection<ChiTietHoaDonNhap> ChiTietHoaDonNhaps { get; set; } = new List<ChiTietHoaDonNhap>();
        public ICollection<ChiTietHoaDonBan> ChiTietHoaDonBans { get; set; } = new List<ChiTietHoaDonBan>();
        
        public ICollection<ChiTietGioHang> ChiTietGioHangs { get; set; } = new List<ChiTietGioHang>();
    }
}
