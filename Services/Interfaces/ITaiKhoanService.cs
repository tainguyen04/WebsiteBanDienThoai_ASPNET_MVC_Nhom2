using QLCHBanDienThoaiMoi.Models;

namespace QLCHBanDienThoaiMoi.Services.Interfaces
{
    public interface ITaiKhoanService
    {
        TaiKhoan? DangNhap(string username, string password);
        Task<bool> DangKyAsync(TaiKhoan tk, KhachHang kh);
        Task<bool> KiemTraTenDangNhap(string username);
    }
}
