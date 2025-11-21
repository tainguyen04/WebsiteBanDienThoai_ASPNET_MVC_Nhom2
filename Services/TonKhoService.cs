using Microsoft.EntityFrameworkCore;
using QLCHBanDienThoaiMoi.Data;
using QLCHBanDienThoaiMoi.Models;
using QLCHBanDienThoaiMoi.Services.Interfaces;

namespace QLCHBanDienThoaiMoi.Services
{
    public class TonKhoService : ITonKhoService
    {
        private readonly ApplicationDbContext _context;
        public TonKhoService(ApplicationDbContext context)
        {
            _context = context;
        }
        //Lấy toàn bộ tồn kho
        public async Task<List<TonKho>> GetAllTonKhoAsync()
        {
            return await _context.TonKho.Include(t => t.SanPham).ToListAsync();
        }

        public async Task<TonKho?> GetTonKhoByIdSanPhamAsync(int id)
        {
            return await _context.TonKho
                                 .Include(t => t.SanPham)
                                 .FirstOrDefaultAsync(t => t.SanPhamId == id);
        }
    }
}
