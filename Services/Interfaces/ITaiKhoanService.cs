using QLCHBanDienThoaiMoi.DTO;
using QLCHBanDienThoaiMoi.Models;

namespace QLCHBanDienThoaiMoi.Services.Interfaces
{
    public interface ITaiKhoanService
    {
        Task<TaiKhoan?> DangNhap(string username, string password);
        Task<bool> DangKyAsync(TaiKhoan tk, KhachHang kh,string sessionId);
        Task<bool> KiemTraTenDangNhap(string username);
        Task<bool> ResetMatKhauAsync(int id,string passWord);
        Task<List<TaiKhoan>> GetAllTaiKhoanAsync();
        Task<TaiKhoan?> GetTaiKhoanByIdAsync(int? id);
        Task<bool> CreateTaiKhoanAsync(TaiKhoan taiKhoan);
        Task<bool> UpdateTaiKhoanAsync(int id, VaiTro newVaiTro);
        Task<bool> DeleteTaiKhoanAsync(int? id);
        Task<bool> LockTaiKhoanAsync(int  id);
        Task<bool> UnlockTaiKhoanAsync(int id);
        Task<bool> ChangePasswordAsync(int id,string oldPassword, string newPassword);
    }
}
