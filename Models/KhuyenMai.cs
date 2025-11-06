using System.ComponentModel.DataAnnotations.Schema;

namespace QLCHBanDienThoaiMoi.Models
{
    public class KhuyenMai
    {
        public int Id { get; set; }
        public string TenKhuyenMai { get; set; } = null!;
        public string? MoTa { get; set; }
        public string? LoaiKhuyenMai { get; set; }
        public DateTime NgayBatDau { get; set; }
        public DateTime NgayKetThuc { get; set; }
        public decimal GiaTri { get; set; }
        [NotMapped]
        public bool HieuLuc => DateTime.Now >= NgayBatDau && DateTime.Now <= NgayKetThuc;
        public ICollection<SanPham> SanPhams { get; set; } = new List<SanPham>();
    }
}
