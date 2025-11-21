using Microsoft.EntityFrameworkCore;
using QLCHBanDienThoaiMoi.Data;
using QLCHBanDienThoaiMoi.Models;

namespace QLCHBanDienThoaiMoi.Services
{
    public class KhachHangService
    {
        private readonly ApplicationDbContext _context;
        public KhachHangService(ApplicationDbContext context)
        {
            _context = context;
        }
        //Lấy tất cả khách hàng
        public async Task<List<KhachHang>> GetAllKhachHangAsync()
        {
            return await _context.KhachHang.ToListAsync();
        }
        //Lấy khách hàng theo Id
        public async Task<KhachHang?> GetKhachHangById(int id)
        {
            return await _context.KhachHang.FirstOrDefaultAsync(kh => kh.Id == id);
        }
        //Tạo mới khách hàng
        public async Task<bool> CreateKhachHangAsync(KhachHang khachHang)
        {
            _context.KhachHang.Add(khachHang);
            return await _context.SaveChangesAsync() > 0;
        }
        //Cập nhật khách hàng
        public async Task<bool> UpdateKhachHangAsync(KhachHang khachHang)
        {
            _context.KhachHang.Update(khachHang);
            return await _context.SaveChangesAsync() > 0;
        }
        //Xóa khách hàng
        public async Task<bool> DeleteKhachHangAsync(int id)
        {
            var khachHang = await _context.KhachHang.FindAsync(id);
            if (khachHang == null)
            {
                return false;
            }
            _context.KhachHang.Remove(khachHang);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
