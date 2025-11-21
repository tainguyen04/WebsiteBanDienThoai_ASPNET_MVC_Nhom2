using Microsoft.EntityFrameworkCore;
using QLCHBanDienThoaiMoi.Data;
using QLCHBanDienThoaiMoi.Models;

namespace QLCHBanDienThoaiMoi.Services
{
    public class NhanVienService
    {
        private readonly ApplicationDbContext _context;
        public NhanVienService(ApplicationDbContext context)
        {
            _context = context;
        }
        //Lấy tất cả nhân viên
        public async Task<List<NhanVien>> GetAllNhanVienAsync()
        {
            return await _context.NhanVien.ToListAsync();
        }
        //Lấy nhân viên theo Id
        public async Task<NhanVien?> GetNhanVienById(int id)
        {
            return await _context.NhanVien.FirstOrDefaultAsync(nv => nv.Id == id);
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
        //Xóa nhân viên
        public async Task<bool> DeleteNhanVienAsync(int id)
        {
            var nhanVien = await _context.NhanVien.FindAsync(id);
            if (nhanVien == null)
            {
                return false;
            }
            _context.NhanVien.Remove(nhanVien);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
