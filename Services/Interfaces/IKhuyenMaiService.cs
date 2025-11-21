using QLCHBanDienThoaiMoi.Models;

namespace QLCHBanDienThoaiMoi.Services.Interfaces
{
    public interface IKhuyenMaiService
    {
        Task<List<KhuyenMai>> GetAllKhuyenMaiAsync();
        Task<KhuyenMai?> GetKhuyenMaiById(int id);
        Task<bool> CreateKhuyenMaiAsync(KhuyenMai khuyenMai);
        Task<bool> UpdateKhuyenMaiAsync(KhuyenMai khuyenMai);
        Task<bool> DeleteKhuyenMaiAsync(int id);
    }
}
