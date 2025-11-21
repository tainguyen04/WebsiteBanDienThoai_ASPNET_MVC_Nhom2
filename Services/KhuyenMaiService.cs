using Microsoft.EntityFrameworkCore;
using QLCHBanDienThoaiMoi.Data;
using QLCHBanDienThoaiMoi.Models;
using QLCHBanDienThoaiMoi.Services.Interfaces;

namespace QLCHBanDienThoaiMoi.Services
{
    public class KhuyenMaiService : IKhuyenMaiService
    {
        private readonly ApplicationDbContext _context;
        public KhuyenMaiService(ApplicationDbContext context)
        {
            _context = context;
        }
        //Lấy tất cả khuyến mãi
        public async Task<List<KhuyenMai>> GetAllKhuyenMaiAsync()
        {
            return await _context.KhuyenMai.ToListAsync();
        }
        //Lấy khuyến mãi theo Id
        public async Task<KhuyenMai?> GetKhuyenMaiById(int id)
        {
            return await _context.KhuyenMai.FirstOrDefaultAsync( km => km.Id == id);
        }
        //Tạo mới khuyến mãi
        public async Task<bool> CreateKhuyenMaiAsync(KhuyenMai khuyenMai)
        {
            _context.KhuyenMai.Add(khuyenMai);
            return  await _context.SaveChangesAsync() > 0;
        }
        //Cập nhật khuyến mãi
        public async Task<bool> UpdateKhuyenMaiAsync(KhuyenMai khuyenMai)
        {
            _context.KhuyenMai.Update(khuyenMai);
            return await _context.SaveChangesAsync() > 0;
        }
        //Xóa khuyến mãi
        public async Task<bool> DeleteKhuyenMaiAsync(int id)
        {
            var khuyenMai = await _context.KhuyenMai.FindAsync(id);
            if (khuyenMai == null)
            {
                return false;
            }
            _context.KhuyenMai.Remove(khuyenMai);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
