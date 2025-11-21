using QLCHBanDienThoaiMoi.Models;

namespace QLCHBanDienThoaiMoi.Services.Interfaces
{
    public interface INhanVienService
    {
        Task<List<NhanVien>> GetAllNhanVienAsync();
        Task<NhanVien?> GetNhanVienById(int id);
        Task<bool> CreateNhanVienAsync(NhanVien nhanVien);
        Task<bool> UpdateNhanVienAsync(NhanVien nhanVien);
        Task<bool> DeleteNhanVienAsync(int id);
    }
}
