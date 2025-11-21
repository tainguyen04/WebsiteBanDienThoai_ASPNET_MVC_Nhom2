using QLCHBanDienThoaiMoi.Models;

namespace QLCHBanDienThoaiMoi.Services.Interfaces
{
    public interface ITonKhoService
    {
        Task<List<TonKho>> GetAllTonKhoAsync();
        Task<TonKho?> GetTonKhoByIdSanPhamAsync(int id);
    }
}
