using Microsoft.EntityFrameworkCore;
using QLCHBanDienThoaiMoi.Data;
using QLCHBanDienThoaiMoi.Models;

namespace QLCHBanDienThoaiMoi.Services
{
    public class TonKhoService
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
    }
}
