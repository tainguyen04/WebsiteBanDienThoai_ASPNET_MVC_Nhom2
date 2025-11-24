using Microsoft.EntityFrameworkCore;
using QLCHBanDienThoaiMoi.Data;
using QLCHBanDienThoaiMoi.Models;
using QLCHBanDienThoaiMoi.Services.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace QLCHBanDienThoaiMoi.Services
{
    public class TaiKhoanService : ITaiKhoanService
    {
        private readonly ApplicationDbContext _context;
        private readonly IGioHangService _gioHangService;

        public TaiKhoanService(ApplicationDbContext context, IGioHangService gioHangService)
        {
            _context = context;
            _gioHangService = gioHangService;
        }

        private static string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return string.Concat(hash.Select(b => b.ToString("x2")));
        }

        public async Task<TaiKhoan?> DangNhap(string username, string password)
        {
            var hashed = HashPassword(password);
            return await _context.TaiKhoan
                .Include(t => t.KhachHang)
                .FirstOrDefaultAsync(t => t.TenDangNhap == username
                                       && t.MatKhau == hashed
                                       && t.TrangThai == TrangThaiTaiKhoan.Active);
        }

        public async Task<bool> DangKyAsync(TaiKhoan tk, KhachHang kh, string sessionId)
        {
            if (await KiemTraTenDangNhap(tk.TenDangNhap))
                return false;

            tk.MatKhau = HashPassword(tk.MatKhau);
            tk.VaiTro = VaiTro.User;
            tk.TrangThai = TrangThaiTaiKhoan.Active;

            // Gán ngược: KhachHang thuộc về TaiKhoan nào
            kh.TaiKhoan = tk; // ← ĐÃ SỬA: Dùng navigation thay vì TaiKhoanId

            await _context.TaiKhoan.AddAsync(tk);
            await _context.KhachHang.AddAsync(kh);

            await _context.SaveChangesAsync(); // Lưu cả 2 cùng lúc

            // Tạo giỏ hàng + gộp session
            await _gioHangService.CreateGioHangAsync(null, kh.Id);
            if (!string.IsNullOrEmpty(sessionId))
                await _gioHangService.MergeCartAsync(sessionId, kh.Id);

            return true;
        }

        public async Task<bool> KiemTraTenDangNhap(string username)
        {
            return await _context.TaiKhoan.AnyAsync(t => t.TenDangNhap == username);
        }

        public async Task<bool> ResetMatKhauAsync(int id, string passWord)
        {
            var tk = await _context.TaiKhoan.FindAsync(id);
            if (tk == null) return false;
            tk.MatKhau = HashPassword(passWord);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<TaiKhoan>> GetAllTaiKhoanAsync()
        {
            return await _context.TaiKhoan
                .Include(t => t.KhachHang)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<TaiKhoan?> GetTaiKhoanByIdAsync(int? id)
        {
            if (!id.HasValue) return null;
            return await _context.TaiKhoan
                .Include(t => t.KhachHang)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<bool> CreateTaiKhoanAsync(TaiKhoan taiKhoan)
        {
            if (await KiemTraTenDangNhap(taiKhoan.TenDangNhap))
                return false;

            taiKhoan.MatKhau = HashPassword(taiKhoan.MatKhau);
            taiKhoan.TrangThai = TrangThaiTaiKhoan.Active;

            if (taiKhoan.VaiTro == VaiTro.User)
            {
                var kh = new KhachHang
                {
                    TenKhachHang = taiKhoan.TenDangNhap,
                    TaiKhoan = taiKhoan  // ← Dùng navigation
                };
                taiKhoan.KhachHang = kh;
            }

            _context.TaiKhoan.Add(taiKhoan);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateTaiKhoanAsync(int id, VaiTro newVaiTro)
        {
            var tk = await _context.TaiKhoan
                .Include(t => t.KhachHang)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (tk == null) return false;
            if (tk.VaiTro == newVaiTro) return true;

            var oldRole = tk.VaiTro;
            tk.VaiTro = newVaiTro;

            if (oldRole == VaiTro.User && newVaiTro == VaiTro.Admin && tk.KhachHang != null)
            {
                _context.KhachHang.Remove(tk.KhachHang);
                tk.KhachHang = null;
            }

            if (oldRole == VaiTro.Admin && newVaiTro == VaiTro.User && tk.KhachHang == null)
            {
                tk.KhachHang = new KhachHang
                {
                    TenKhachHang = tk.TenDangNhap,
                    TaiKhoan = tk
                };
            }

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteTaiKhoanAsync(int? id)
        {
            if (!id.HasValue) return false;

            var tk = await _context.TaiKhoan
                .Include(t => t.KhachHang)
                .FirstOrDefaultAsync(t => t.Id == id.Value);

            if (tk == null) return false;

            if (tk.KhachHang != null)
                _context.KhachHang.Remove(tk.KhachHang);

            _context.TaiKhoan.Remove(tk);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> LockTaiKhoanAsync(int id)
        {
            var tk = await _context.TaiKhoan.FindAsync(id);
            if (tk == null) return false;
            tk.TrangThai = TrangThaiTaiKhoan.Locked;
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UnlockTaiKhoanAsync(int id)
        {
            var tk = await _context.TaiKhoan.FindAsync(id);
            if (tk == null) return false;
            tk.TrangThai = TrangThaiTaiKhoan.Active;
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> ChangePasswordAsync(int id, string oldPassword, string newPassword)
        {
            var tk = await _context.TaiKhoan.FindAsync(id);
            if (tk == null) return false;

            if (tk.MatKhau != HashPassword(oldPassword))
                return false;

            tk.MatKhau = HashPassword(newPassword);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}