using Microsoft.EntityFrameworkCore;
using QLCHBanDienThoaiMoi.Data;
using QLCHBanDienThoaiMoi.Models;
using QLCHBanDienThoaiMoi.Services.Interfaces;

namespace QLCHBanDienThoaiMoi.Services
{
    public class PhieuBaoHanhService : IPhieuBaoHanhService
    {
        private readonly ApplicationDbContext _context;

        public PhieuBaoHanhService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Lấy toàn bộ phiếu bảo hành
        public async Task<List<PhieuBaoHanh>> GetAllAsync()
        {
            return await _context.PhieuBaoHanh
                .Include(p => p.ChiTietHoaDonBan)
                    .ThenInclude(ct => ct!.SanPham)
                .Include(p => p.ChiTietHoaDonBan.HoaDonBan)
                .AsNoTracking()
                .ToListAsync();
        }

        // Lấy phiếu theo khóa kép
        public async Task<PhieuBaoHanh?> GetByIdAsync(int hoaDonBanId, int sanPhamId)
        {
            return await _context.PhieuBaoHanh
                .Include(p => p.ChiTietHoaDonBan)
                    .ThenInclude(ct => ct!.SanPham)
                .Include(p => p.ChiTietHoaDonBan.HoaDonBan)
                .FirstOrDefaultAsync(p =>
                    p.HoaDonBanId == hoaDonBanId &&
                    p.SanPhamId == sanPhamId
                );
        }

        // Tạo mới phiếu bảo hành
        public async Task<bool> CreateAsync(PhieuBaoHanh phieu)
        {
            _context.PhieuBaoHanh.Add(phieu);
            return await _context.SaveChangesAsync() > 0;
        }

        // Cập nhật phiếu bảo hành
        public async Task<bool> UpdateAsync(PhieuBaoHanh phieu)
        {
            var existing = await _context.PhieuBaoHanh
                .FirstOrDefaultAsync(p =>
                    p.HoaDonBanId == phieu.HoaDonBanId &&
                    p.SanPhamId == phieu.SanPhamId);

            if (existing == null)
                return false;

            _context.Entry(existing).CurrentValues.SetValues(phieu);

            return await _context.SaveChangesAsync() > 0;
        }

        // Xóa phiếu bảo hành
        public async Task<bool> DeleteAsync(int hoaDonBanId, int sanPhamId)
        {
            var phieu = await _context.PhieuBaoHanh
                .FirstOrDefaultAsync(p =>
                    p.HoaDonBanId == hoaDonBanId &&
                    p.SanPhamId == sanPhamId);

            if (phieu == null)
                return false;

            _context.PhieuBaoHanh.Remove(phieu);
            return await _context.SaveChangesAsync() > 0;
        }

        // Lấy danh sách hóa đơn (dùng cho dropdown)
        public async Task<List<HoaDonBan>> GetHoaDonBanListAsync()
        {
            return await _context.HoaDonBan
                .AsNoTracking()
                .OrderByDescending(h => h.Id)
                .ToListAsync();
        }

        // Lấy danh sách sản phẩm (dropdown)
        public async Task<List<SanPham>> GetSanPhamListAsync()
        {
            return await _context.SanPham
                .AsNoTracking()
                .OrderBy(s => s.TenSanPham)
                .ToListAsync();
        }

        // Lấy phiếu bảo hành theo trạng thái
        public async Task<List<PhieuBaoHanh>> GetByTrangThaiAsync(TrangThaiBaoHanh trangThai)
        {
            return await _context.PhieuBaoHanh
                .Where(p => p.TrangThai == trangThai)
                .Include(p => p.ChiTietHoaDonBan)
                .ThenInclude(ct => ct!.SanPham)
                .AsNoTracking()
                .ToListAsync();
        }

        // Kiểm tra phiếu có hết hạn hay chưa
        public async Task<bool> KiemTraHetHanAsync(int hoaDonBanId, int sanPhamId)
        {
            var phieu = await _context.PhieuBaoHanh
                .AsNoTracking()
                .FirstOrDefaultAsync(p =>
                    p.HoaDonBanId == hoaDonBanId &&
                    p.SanPhamId == sanPhamId);

            if (phieu == null)
                return false;

            return phieu.NgayHetHan < DateTime.Now;
        }
    }
}
