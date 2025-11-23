using QLCHBanDienThoaiMoi.Models;

namespace QLCHBanDienThoaiMoi.Services.Interfaces
{
    public interface IDanhMucSanPhamService
    {
        Task<List<DanhMucSanPham>> GetAllDanhMucSanPhamAsync();
        Task<bool> AddDanhMucSanPhamAsync(DanhMucSanPham danhMuc);
        Task<bool> UpdateDanhMucSanPhamAsync(DanhMucSanPham danhMuc);
        Task<bool> DeleteDanhMucSanPhamAsync(int id);
        Task<DanhMucSanPham?> GetDanhMucSanPhamByIdAsync(int id);
        Task<IEnumerable<DanhMucSanPham>> SearchAsync(string keyword);
        Task<IEnumerable<DanhMucSanPham>> GetAllAsync();
    }
}
