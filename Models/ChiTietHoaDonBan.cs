using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLCHBanDienThoaiMoi.Models
{
    public class ChiTietHoaDonBan
    {
        public int Id { get; set; }

        public int HoaDonBanId { get;set; }
        
       
        public int SanPhamId { get;set; }
        
        
        public int SoLuong { get; set; }
        public int GiaBan { get; set; }
        public decimal? KhuyenMai { get; set; }

        public PhieuBaoHanh? PhieuBaoHanh { get; set; }
        public HoaDonBan? HoaDonBan { get; set; } 
        public SanPham? SanPham { get; set; }
        [NotMapped]
        public decimal ThanhTien => SoLuong * GiaBan * (1 - (KhuyenMai ?? 0) / 100);
    }
}
