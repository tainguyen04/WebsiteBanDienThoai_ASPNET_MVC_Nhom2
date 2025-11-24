using Microsoft.EntityFrameworkCore;
using QLCHBanDienThoaiMoi.Data;
using QLCHBanDienThoaiMoi.DTO;
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

        // ============================
        // 🔐 Hàm mã hóa mật khẩu
        // ============================
        public static string HashPasswordSHA256(string password)
        {
            using (var sha = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hash = sha.ComputeHash(bytes);

                // Convert to hex
                StringBuilder sb = new StringBuilder();
                foreach (var b in hash)
                    sb.Append(b.ToString("x2"));

                return sb.ToString();
            }
        }

        // ============================
        // 🔐 Đăng nhập
        // ============================
        // Thay thế hoàn toàn phương thức DangNhap cũ
        public async Task<TaiKhoan?> DangNhap(string username, string password)
        {
            string hashed = HashPasswordSHA256(password);
            return await _context.TaiKhoan
                .Include(t => t.KhachHang)
                .Include(t => t.NhanVien)
                .FirstOrDefaultAsync(t => t.TenDangNhap == username
                                       && t.MatKhau == hashed
                                       && t.TrangThai == TrangThaiTaiKhoan.Active);
        }


        // ============================
        // 📝 Đăng ký tài khoản khách hàng
        // ============================
        public async Task<bool> DangKyAsync(TaiKhoan tk, KhachHang kh, string sessionId)
        {
            if (await KiemTraTenDangNhap(tk.TenDangNhap))
                return false;

            tk.MatKhau = HashPasswordSHA256(tk.MatKhau);
            tk.VaiTro = VaiTro.User;

            await _context.TaiKhoan.AddAsync(tk);
            await _context.SaveChangesAsync();

            // Gán tài khoản vào khách hàng
            kh.TaiKhoan = tk;
            await _context.KhachHang.AddAsync(kh);
            await _context.SaveChangesAsync();
            //Tạo giỏ hàng
            await _gioHangService.CreateGioHangAsync(null, kh.Id);
            if (string.IsNullOrEmpty(sessionId))
                await _gioHangService.MergeCartAsync(sessionId, kh.Id);
            return true;
        }

        // ============================
        // 🔍 Kiểm tra tên đăng nhập
        // ============================
        public async Task<bool> KiemTraTenDangNhap(string username)
        {
            return await _context.TaiKhoan
                .AnyAsync(x => x.TenDangNhap == username);
        }

        public async Task<List<TaiKhoan>> GetAllTaiKhoanAsync()
        {
            return await _context.TaiKhoan
                                .Include(kh => kh.KhachHang)
                                .Include(nv => nv.NhanVien)
                                .ToListAsync();
        }

        public async Task<TaiKhoan?> GetTaiKhoanByIdAsync(int? id)
        {
            if (id == null) return null;
            return await _context.TaiKhoan
                                .Include(kh => kh.KhachHang)
                                .Include(nv => nv.NhanVien)
                                .FirstOrDefaultAsync(tk => tk.Id == id);
        }

        public async Task<bool> CreateTaiKhoanAsync(TaiKhoan taiKhoan)
        {
            try
            {

                var existingTaiKhoan = await _context.TaiKhoan.AnyAsync(tk => tk.TenDangNhap == taiKhoan.TenDangNhap);
                if (existingTaiKhoan) return false;
                taiKhoan.MatKhau = HashPasswordSHA256(taiKhoan.MatKhau);
                //if (taiKhoan.VaiTro == VaiTro.User)
                //    taiKhoan.KhachHang = new KhachHang();
                //else if (taiKhoan.VaiTro == VaiTro.Admin)
                //    taiKhoan.NhanVien = new NhanVien();
                if (taiKhoan.VaiTro == VaiTro.User)
                {
                    taiKhoan.KhachHang = new KhachHang { TenKhachHang = taiKhoan.TenDangNhap, TaiKhoan = taiKhoan };
                }
                else // Staff hoặc Admin đều cần có NhanVien
                {
                    taiKhoan.NhanVien = new NhanVien
                    {
                        TenNhanVien = taiKhoan.TenDangNhap + (taiKhoan.VaiTro == VaiTro.Admin ? " (Quản trị)" : " (Nhân viên)"),
                        TaiKhoan = taiKhoan
                    };
                }

                _context.TaiKhoan.Add(taiKhoan);
                return await _context.SaveChangesAsync() > 0;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateTaiKhoanAsync(int id, VaiTro newVaiTro)
        {
            var tk = await GetTaiKhoanByIdAsync(id);
            if (tk == null || tk.VaiTro == newVaiTro) return tk != null;

            var oldRole = tk.VaiTro;
            tk.VaiTro = newVaiTro;

            // Nếu đổi nhóm: User ↔ (Staff/Admin)
            if ((oldRole == VaiTro.User) != (newVaiTro == VaiTro.User))
            {
                // Xóa cái cũ
                if (tk.KhachHang != null) { _context.KhachHang.Remove(tk.KhachHang); tk.KhachHang = null; }
                if (tk.NhanVien != null) { _context.NhanVien.Remove(tk.NhanVien); tk.NhanVien = null; }

                // Tạo cái mới
                if (newVaiTro == VaiTro.User)
                    tk.KhachHang = new KhachHang { TenKhachHang = tk.TenDangNhap, TaiKhoan = tk };
                else
                    tk.NhanVien = new NhanVien
                    {
                        TenNhanVien = tk.TenDangNhap + (newVaiTro == VaiTro.Admin ? " (Quản trị)" : " (Nhân viên)"),
                        TaiKhoan = tk
                    };
            }
            // Nếu cùng nhóm nhân viên (Staff ↔ Admin) → chỉ cập nhật tên
            else if (tk.NhanVien != null && (oldRole == VaiTro.Staff || oldRole == VaiTro.Admin))
            {
                tk.NhanVien.TenNhanVien = tk.TenDangNhap + (newVaiTro == VaiTro.Admin ? " (Quản trị)" : " (Nhân viên)");
            }

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteTaiKhoanAsync(int? id)
        {
            var taiKhoan = await GetTaiKhoanByIdAsync(id);
            if (taiKhoan == null) return false;
            if (taiKhoan.NhanVien != null)
                _context.NhanVien.Remove(taiKhoan.NhanVien);
            else if (taiKhoan.KhachHang != null)
                _context.KhachHang.Remove(taiKhoan.KhachHang);

            _context.TaiKhoan.Remove(taiKhoan);
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

        public async Task<bool> ResetMatKhauAsync(int id, string passWord)
        {
            var tk = await _context.TaiKhoan.FindAsync(id);
            if (tk == null) return false;
            tk.MatKhau = HashPasswordSHA256(passWord);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}