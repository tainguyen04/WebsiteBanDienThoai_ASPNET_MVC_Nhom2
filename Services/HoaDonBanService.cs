using Microsoft.EntityFrameworkCore;
using QLCHBanDienThoaiMoi.Data;
using QLCHBanDienThoaiMoi.Models;

namespace QLCHBanDienThoaiMoi.Services
{
    public class HoaDonBanService
    {
        private readonly ApplicationDbContext _context;
        
        public HoaDonBanService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<HoaDonBan?> GetHoaDonBanAsync(int id)
        {
            return  await _context.HoaDonBan
                    .Include(hd => hd.KhachHang)
                    .Include(hd => hd.NhanVien)
                    .Include(ct => ct.ChiTietHoaDonBans)
                    .FirstOrDefaultAsync( hd => hd.Id == id);
        }
        public async Task<List<HoaDonBan>> GetAllHoaDonBanAsync()
        {
            return await _context.HoaDonBan
                    .Include(hd => hd.KhachHang)
                    .Include(hd => hd.NhanVien)
                    .Include(ct => ct.ChiTietHoaDonBans)
                    .ToListAsync();
        }
        public async Task<bool> CreateHoaDonBanAsync(HoaDonBan hoaDonBan,List<ChiTietHoaDonBan> chiTiet)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                hoaDonBan.TongTien = chiTiet.Sum(ct => (int)ct.ThanhTien);
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return false;
            }
            _context.HoaDonBan.Add(hoaDonBan);
            var hd = await _context.SaveChangesAsync();
            return hd > 0;
        }
        public async Task<bool> DeleteHoaDonBanAsync(int id)
        {
            var hoaDonBan = await _context.HoaDonBan.FindAsync(id);
            if (hoaDonBan == null)
            {
                return false;
            }
            _context.HoaDonBan.Remove(hoaDonBan);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
        public async Task<bool> UpdateTrangThaiAsync(int id,TrangThaiHoaDon trangThai)
        {
            var existingHoaDonBan = await GetHoaDonBanAsync(id);
            if (existingHoaDonBan == null)
                return false;
            existingHoaDonBan.TrangThai = trangThai;
            _context.HoaDonBan.Update(existingHoaDonBan);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
