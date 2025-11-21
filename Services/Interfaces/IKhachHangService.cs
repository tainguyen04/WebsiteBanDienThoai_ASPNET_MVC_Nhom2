using QLCHBanDienThoaiMoi.Models;

namespace QLCHBanDienThoaiMoi.Services.Interfaces
{
    public interface IKhachHangService
    {
        Task<List<KhachHang>> GetAllKhachHangAsync();
        Task<KhachHang?> GetKhachHangById(int id);
        Task<bool> CreateKhachHangAsync(KhachHang khachHang);
        Task<bool> UpdateKhachHangAsync(KhachHang khachHang);
        Task<bool> DeleteKhachHangAsync(int id);
    }
}
