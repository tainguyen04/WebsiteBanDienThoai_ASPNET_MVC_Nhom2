using QLCHBanDienThoaiMoi.DTO;
using QLCHBanDienThoaiMoi.Models;

namespace QLCHBanDienThoaiMoi.Services.Interfaces
{
    public interface IHoaDonBanService
    {
        Task<HoaDonBan?> GetHoaDonBanAsync(int id);
        Task<List<HoaDonBan>> GetAllHoaDonBanAsync();
        Task<bool> CreateHoaDonBanAsync(HoaDonBan hoaDonBan, List<ChiTietHoaDonBan> chiTiet);
        Task<bool> DeleteHoaDonBanAsync(int id);
        Task<bool> UpdateHoaDonBanAsync(UpdateHoaDonBanDTO hoaDonBanDTO);
        Task<List<HoaDonBan>> GetHoaDonBanByUserAsync(int userId);
        Task<bool> UpdateDiaChiNhanHangAsync(int id,int userId,string diaChiNhanHang);
    }
}
