using Microsoft.EntityFrameworkCore;
using QLCHBanDienThoaiMoi.Data;
using QLCHBanDienThoaiMoi.Models;
using QLCHBanDienThoaiMoi.Services.Interfaces;

namespace QLCHBanDienThoaiMoi.Services
{
    public class NhaCungCapService : INhaCungCapService
    {
        private readonly ApplicationDbContext _context;
        public NhaCungCapService(ApplicationDbContext context)
        {
            _context = context;
        }
        //Lấy tất cả nhà cung cấp
        public async Task<List<NhaCungCap>> GetAllNhaCungCapAsync()
        {
            return await _context.NhaCungCap.ToListAsync();
        }
        //Lấy nhà cung cấp theo Id
        public async Task<NhaCungCap?> GetNhaCungCapById(int id)
        {
            return await _context.NhaCungCap.FindAsync(id);
        }
        //Tạo mới nhà cung cấp
        public async Task<bool> CreateNhaCungCapAsync(NhaCungCap nhaCungCap)
        {
            _context.NhaCungCap.Add(nhaCungCap);
            return await _context.SaveChangesAsync() > 0;
        }
        //Cập nhật nhà cung cấp
        public async Task<bool> UpdateNhaCungCapAsync(NhaCungCap nhaCungCap)
        {
            _context.NhaCungCap.Update(nhaCungCap);
            return await _context.SaveChangesAsync() > 0;
        }
        //Xóa nhà cung cấp
        public async Task<bool> DeleteNhaCungCapAsync(int id)
        {
            var nhaCungCap = await _context.NhaCungCap.FindAsync(id);
            if (nhaCungCap == null)
            {
                return false;
            }
            _context.NhaCungCap.Remove(nhaCungCap);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
