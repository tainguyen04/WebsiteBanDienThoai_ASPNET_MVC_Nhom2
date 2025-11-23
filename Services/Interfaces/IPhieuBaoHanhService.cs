using QLCHBanDienThoaiMoi.Models;

namespace QLCHBanDienThoaiMoi.Services.Interfaces
{
    public interface IPhieuBaoHanhService
    {
        // CRUD
        Task<List<PhieuBaoHanh>> GetAllAsync();
        Task<PhieuBaoHanh?> GetByIdAsync(int hoaDonBanId, int sanPhamId);
        Task<bool> CreateAsync(PhieuBaoHanh phieu);
        Task<bool> UpdateAsync(PhieuBaoHanh phieu);
        Task<bool> DeleteAsync(int hoaDonBanId, int sanPhamId);

        // Dùng cho dropdown
        Task<List<HoaDonBan>> GetHoaDonBanListAsync();
        Task<List<SanPham>> GetSanPhamListAsync();

        // Chức năng mở rộng
        Task<List<PhieuBaoHanh>> GetByTrangThaiAsync(TrangThaiBaoHanh trangThai);
        Task<bool> KiemTraHetHanAsync(int hoaDonBanId, int sanPhamId);
    }
}
