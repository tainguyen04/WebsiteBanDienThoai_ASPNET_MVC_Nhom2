
using Humanizer;
using Microsoft.EntityFrameworkCore;
using QLCHBanDienThoaiMoi.Data;
using QLCHBanDienThoaiMoi.DTO;
using QLCHBanDienThoaiMoi.Models;
using QLCHBanDienThoaiMoi.Services.Interfaces;

namespace QLCHBanDienThoaiMoi.Services
{
    public class HoaDonBanService : IHoaDonBanService
    {
        private readonly ApplicationDbContext _context;

        public HoaDonBanService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<HoaDonBan?> GetHoaDonBanAsync(int id)
        {
            return await _context.HoaDonBan
                    .Include(hd => hd.KhachHang)
                    .Include(hd => hd.NhanVien)
                    .Include(ct => ct.ChiTietHoaDonBans)
                    .FirstOrDefaultAsync(hd => hd.Id == id);
        }
        public async Task<List<HoaDonBan>> GetAllHoaDonBanAsync()
        {
            return await _context.HoaDonBan
                    .Include(hd => hd.KhachHang)
                    .Include(hd => hd.NhanVien)
                    .Include(ct => ct.ChiTietHoaDonBans)
                    .ToListAsync();
        }
        public async Task<bool> CreateHoaDonBanAsync(HoaDonBan hoaDonBan, List<ChiTietHoaDonBan> chiTiet)
        {
            if (hoaDonBan == null || chiTiet == null || !chiTiet.Any())
            {
                return false;
            }
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                hoaDonBan.TongTien = (int)chiTiet.Sum(ct => ct.SoLuong * ct.GiaBan);
                _context.HoaDonBan.Add(hoaDonBan);
                await _context.SaveChangesAsync();
                foreach (var ct in chiTiet)
                {
                    ct.HoaDonBanId = hoaDonBan.Id;
                    ct.HoaDonBan = null;
                }
                _context.ChiTietHoaDonBan.AddRange(chiTiet);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                Console.WriteLine(ex.StackTrace);
                return false;
            }
        }
        public async Task<bool> DeleteHoaDonBanAsync(int id)
        {
            var hoaDonBan = await _context.HoaDonBan
                                .Include(ct => ct.ChiTietHoaDonBans)
                                .FirstOrDefaultAsync(hd => hd.Id == id);

            if (hoaDonBan == null)
            {
                return false;
            }
            if (hoaDonBan.ChiTietHoaDonBans.Any())
            {
                _context.ChiTietHoaDonBan.RemoveRange(hoaDonBan.ChiTietHoaDonBans);
            }
            _context.HoaDonBan.Remove(hoaDonBan);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
        public async Task<bool> UpdateHoaDonBanAsync(UpdateHoaDonBanDTO hoaDonBanDTO)
        {
            var hoaDonBan = await _context.HoaDonBan.FindAsync(hoaDonBanDTO.Id);
            if (hoaDonBan == null) return false;
            hoaDonBan.TrangThai = hoaDonBanDTO.TrangThai;
            hoaDonBan.DiaChiNhanHang = hoaDonBanDTO.DiaChiNhanHang;
            _context.HoaDonBan.Update(hoaDonBan);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<HoaDonBan>> GetHoaDonBanByUserAsync(int userId)
        {
            return await _context.HoaDonBan
                                .Where(hd => hd.KhachHangId == userId)
                                .ToListAsync();
        }

        public async Task<bool> UpdateDiaChiNhanHangAsync(int id, int userId, string diaChiNhanHang)
        {
            var hoaDon = await _context.HoaDonBan.FindAsync(id);
            if(hoaDon == null) return false;
            if(hoaDon.KhachHangId == userId) return false;
            hoaDon.DiaChiNhanHang = diaChiNhanHang;
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
