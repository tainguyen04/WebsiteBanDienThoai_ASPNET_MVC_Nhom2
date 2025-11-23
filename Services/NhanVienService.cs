using Microsoft.EntityFrameworkCore;
using QLCHBanDienThoaiMoi.Data;
using QLCHBanDienThoaiMoi.Models;
using QLCHBanDienThoaiMoi.Services.Interfaces;

namespace QLCHBanDienThoaiMoi.Services
{
    public class NhanVienService : INhanVienService
    {
        private readonly ApplicationDbContext _context;
        public NhanVienService(ApplicationDbContext context)
        {
            _context = context;
        }
        //Lấy tất cả nhân viên
        public async Task<List<NhanVien>> GetAllNhanVienAsync()
        {
            return await _context.NhanVien
                                .Include(tk => tk.TaiKhoan)
                                .ToListAsync();
        }
        //Lấy nhân viên theo Id
        public async Task<NhanVien?> GetNhanVienById(int id)
        {
            return await _context.NhanVien.FindAsync(id);
        }
        //Tạo mới nhân viên
        public async Task<bool> CreateNhanVienAsync(NhanVien nhanVien)
        {
            _context.NhanVien.Add(nhanVien);
            return await _context.SaveChangesAsync() > 0;
        }
        //Cập nhật nhân viên
        public async Task<bool> UpdateNhanVienAsync(NhanVien nhanVien)
        {
            _context.NhanVien.Update(nhanVien);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
