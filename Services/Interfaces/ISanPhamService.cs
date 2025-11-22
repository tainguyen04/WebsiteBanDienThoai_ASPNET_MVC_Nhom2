using QLCHBanDienThoaiMoi.DTO;
using QLCHBanDienThoaiMoi.Models;

namespace QLCHBanDienThoaiMoi.Services.Interfaces
{
    public interface ISanPhamService
    {
        Task<List<SanPham>> GetSanPhamsAsync();
        Task<List<SanPhamDTO>> GetSanPhamHomePageAsync();
        Task<bool> CreateSanPhamAsync(IFormFile file, SanPham sanPham);
        Task<SanPham?> GetSanPhamByIdAsync(int? id);
        Task<bool> UpdateSanPhamAsync(IFormFile? file, SanPham sanPham);
        Task<SanPham?> DeleteByIdAsync(int? id);
        Task<List<DanhMucSanPham>> GetAllDanhMucAsync();
        Task<List<KhuyenMaiDTO>> GetAllKhuyenMaiAsync();
        Task<string> UploadFileAsync(IFormFile file);
        Task<IEnumerable<SanPham>> SearchAsync(string keyword);

    }
}
