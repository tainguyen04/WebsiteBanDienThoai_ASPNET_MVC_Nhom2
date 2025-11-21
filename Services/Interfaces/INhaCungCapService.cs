using QLCHBanDienThoaiMoi.Models;

namespace QLCHBanDienThoaiMoi.Services.Interfaces
{
    public interface INhaCungCapService
    {
        Task<List<NhaCungCap>> GetAllNhaCungCapAsync();
        Task<NhaCungCap?> GetNhaCungCapById(int id);
        Task<bool> CreateNhaCungCapAsync(NhaCungCap nhaCungCap);
        Task<bool> UpdateNhaCungCapAsync(NhaCungCap nhaCungCap);
        Task<bool> DeleteNhaCungCapAsync(int id);
    }
}
