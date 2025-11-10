using System.ComponentModel.DataAnnotations.Schema;

namespace QLCHBanDienThoaiMoi.Models
{
    public class HoaDonNhap
    {
        public int Id { get;set; }
        public DateTime NgayLap { get; set; } =  DateTime.Now;
        public int NhaCungCapId { get; set; }
        public int TongTien { get; set; }
        public NhaCungCap NhaCungCap { get; set; } = null!;
        public ICollection<ChiTietHoaDonNhap> ChiTietHoaDonNhaps { get; set; } = new List<ChiTietHoaDonNhap>();
        
    }
}
