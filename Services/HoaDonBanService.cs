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
                    .Include(ct => ct.ChiTietHoaDonBans)
                    .FirstOrDefaultAsync( hd => hd.Id == id);
        }
        public async Task<bool> CreateHoaDonBanAsync(HoaDonBan hoaDonBan)
        {
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
        public async Task<bool> UpdateHoaDonBanAsync(int id)
        {
            var existingHoaDonBan = await _context.HoaDonBan.FindAsync(id);
            if (existingHoaDonBan == null)
                return false;
            _context.HoaDonBan.Update(existingHoaDonBan);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
