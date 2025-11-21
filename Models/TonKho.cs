namespace QLCHBanDienThoaiMoi.Models
{
    public class TonKho
    {
        public int Id { get; set; }
        public int SanPhamId { get; set; }
        public int SoLuong { get; set; }
        public SanPham? SanPham { get; set; }
    }
}
