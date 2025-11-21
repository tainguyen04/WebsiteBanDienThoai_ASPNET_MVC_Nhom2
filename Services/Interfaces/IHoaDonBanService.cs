using QLCHBanDienThoaiMoi.Models;

namespace QLCHBanDienThoaiMoi.Services.Interfaces
{
    public interface IHoaDonBanService
    {
        Task<HoaDonBan?> GetHoaDonBanAsync(int id);
        Task<List<HoaDonBan>> GetAllHoaDonBanAsync();
        Task<bool> CreateHoaDonBanAsync(HoaDonBan hoaDonBan, List<ChiTietHoaDonBan> chiTiet);
        Task<bool> DeleteHoaDonBanAsync(int id);
        Task<bool> UpdateTrangThaiAsync(int id, TrangThaiHoaDon trangThai)
    }
}
