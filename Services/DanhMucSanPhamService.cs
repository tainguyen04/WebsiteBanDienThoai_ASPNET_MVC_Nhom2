using Microsoft.EntityFrameworkCore;
using QLCHBanDienThoaiMoi.Data;
using QLCHBanDienThoaiMoi.Models;
using QLCHBanDienThoaiMoi.Services.Interfaces;

namespace QLCHBanDienThoaiMoi.Services
{
    public class DanhMucSanPhamService : IDanhMucSanPhamService
    {
        private readonly ApplicationDbContext _context;
        public DanhMucSanPhamService(ApplicationDbContext context)
        {
            _context = context;
        }
        // Lấy tất cả danh mục sản phẩm
        public async Task<List<DanhMucSanPham>> GetAllDanhMucSanPhamAsync()
        {
            return await _context.DanhMucSanPham.ToListAsync();
        }
        // Thêm danh mục sản phẩm mới
        public async Task<bool> AddDanhMucSanPhamAsync(DanhMucSanPham danhMuc)
        {
            _context.DanhMucSanPham.Add(danhMuc);
            return await _context.SaveChangesAsync() > 0;
        }
        // Cập nhật danh mục sản phẩm
        public async Task<bool> UpdateDanhMucSanPhamAsync(DanhMucSanPham danhMuc)
        {
            _context.DanhMucSanPham.Update(danhMuc);
            return await _context.SaveChangesAsync() > 0;
        }
        // Xóa danh mục sản phẩm
        public async Task<bool> DeleteDanhMucSanPhamAsync(int id)
        {
            var danhMuc = await _context.DanhMucSanPham.FindAsync(id);
            if (danhMuc == null)
                return false;
            _context.DanhMucSanPham.Remove(danhMuc);
            return await _context.SaveChangesAsync() > 0;
        }
        // Lấy danh mục sản phẩm theo ID
        public async Task<DanhMucSanPham?> GetDanhMucSanPhamByIdAsync(int id)
        {
            return await _context.DanhMucSanPham.FirstOrDefaultAsync(dm => dm.Id == id);
        }
        public async Task<IEnumerable<DanhMucSanPham>> SearchAsync(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return await _context.DanhMucSanPham.ToListAsync();

            keyword = keyword.Trim().ToLower();

            return await _context.DanhMucSanPham
                .Where(x => x.TenDanhMuc.ToLower().Contains(keyword))
                .ToListAsync();
        }
        public async Task<IEnumerable<DanhMucSanPham>> GetAllAsync()
        {
            return await _context.DanhMucSanPham.ToListAsync();
        }
    }
}
