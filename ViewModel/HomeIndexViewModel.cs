using QLCHBanDienThoaiMoi.DTO;
using QLCHBanDienThoaiMoi.Models;

namespace QLCHBanDienThoaiMoi.ViewModel
{
    public class HomeIndexViewModel
    {
        public List<SanPhamDTO> SanPham { get; set; } = new();
        public List<DanhMucSanPham> DanhMucSanPham { get; set; } = new();
    }
}
