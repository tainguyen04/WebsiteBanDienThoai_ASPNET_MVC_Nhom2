using QLCHBanDienThoaiMoi.DTO;
using QLCHBanDienThoaiMoi.Models;

namespace QLCHBanDienThoaiMoi.Services.Interfaces
{
    public interface IGioHangService
    {
        Task<GioHang?> GetGioHangEntityAsync(string? sessionId, int? khachHangId);
        Task<List<ChiTietGioHangDTO>> GetGioHangAsync(string? sessionId, int? khachHangId);
        Task<bool> CreateGioHangAsync(string? sessionId, int? khachHangId);
        Task<bool> AddToCardAsync(string? sessionId, int sanPhamId, int? khachHangId, int soLuong = 1);
        Task<bool> DeletedSanPhamFromGioHangAsync(string? sessionId, int? khachHangId, int sanPhamId);
        Task<bool> MergeCartAsync(string? sessionId, int? khachHangId);

    }
}
