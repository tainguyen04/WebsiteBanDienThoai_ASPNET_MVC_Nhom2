using Microsoft.EntityFrameworkCore;
using QLCHBanDienThoaiMoi.Data;
using QLCHBanDienThoaiMoi.Models;
using QLCHBanDienThoaiMoi.Services.Interfaces;

namespace QLCHBanDienThoaiMoi.Services
{
    public class KhachHangService : IKhachHangService
    {
        private readonly ApplicationDbContext _context;
        public KhachHangService(ApplicationDbContext context)
        {
            _context = context;
        }
        //Lấy tất cả khách hàng
        public async Task<List<KhachHang>> GetAllKhachHangAsync()
        {
            return await _context.KhachHang
                                .Include(tk => tk.TaiKhoan)
                                .ToListAsync();
        }
        //Lấy khách hàng theo Id
        public async Task<KhachHang?> GetKhachHangById(int id)
        {
            return await _context.KhachHang
                                .AsNoTracking()
                                .FirstOrDefaultAsync(kh => kh.Id == id);
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
    }
}
