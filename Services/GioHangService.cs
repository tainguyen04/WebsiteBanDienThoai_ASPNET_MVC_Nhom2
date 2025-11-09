using Microsoft.EntityFrameworkCore;
using QLCHBanDienThoaiMoi.Data;
using QLCHBanDienThoaiMoi.Models;

namespace QLCHBanDienThoaiMoi.Services
{
    public class GioHangService
    {
        private readonly ApplicationDbContext _context;
        public GioHangService(ApplicationDbContext context)
        {
            _context = context;
        }
        // Lấy giỏ hàng theo sessionId hoặc khachHangId
        public async Task<GioHang?> GetGioHangAsync(string? sessionId, int? khachHangId)
        {
            var gh = _context.GioHang
                            .Include(g => g.ChiTietGioHangs)
                                .ThenInclude(ct => ct.SanPham);
            if(khachHangId.HasValue)
            {
                return await gh.FirstOrDefaultAsync(g => g.KhachHangId == khachHangId);
            }
            if(!string.IsNullOrEmpty(sessionId))
            {
                return await gh.FirstOrDefaultAsync(g => g.SessionId == sessionId);
            }
            return null;
        }
        // Tạo giỏ hàng mới
        public async Task<bool> CreateGioHangAsync(string? sessionId, int? khachHangId)
        {
            if(string.IsNullOrEmpty(sessionId) && !khachHangId.HasValue)
            {
                throw new ArgumentException("Phải có sessionId hoặc khachhangId");
            }
            var existingGioHang = await GetGioHangAsync(sessionId, khachHangId);
            if (existingGioHang != null)
            {
                return false;
            }
            try
            {
                var gioHang = new GioHang
                {
                    SessionId = sessionId,
                    KhachHangId = khachHangId,
                    NgayTao = DateTime.Now
                };
                await _context.GioHang.AddAsync(gioHang);
                return await _context.SaveChangesAsync() > 0;   
            }
            catch(DbUpdateException ex)
            {
                throw new Exception("Lỗi khi tạo giỏ hàng trong database", ex);
            }
        }
        // Thêm sản phẩm vào giỏ hàng
        public async Task<bool> AddToCardAsync(string? sessionId, int sanPhamId, int? khachHangId, int soLuong = 1)
        {
            var sanPham = await _context.SanPham
                                        .AsNoTracking()
                                        .Include(s => s.KhuyenMai)
                                        .FirstOrDefaultAsync(s => s.Id == sanPhamId);
            if (sanPham == null) throw new ArgumentException("Sản phẩm không tồn tại");

            var gioHang = await _context.GioHang
                                        .FirstOrDefaultAsync(g =>
                                            (sessionId != null && g.SessionId == sessionId) ||
                                            (khachHangId.HasValue && g.KhachHangId == khachHangId));

            if (gioHang == null)
            {
                gioHang = new GioHang
                {
                    SessionId = sessionId,
                    KhachHangId = khachHangId,
                    NgayTao = DateTime.Now,
                    ChiTietGioHangs = new List<ChiTietGioHang>()
                };
                await _context.GioHang.AddAsync(gioHang);
                await _context.SaveChangesAsync();
            }

            // Fix for the line causing CS0029, CS1003, and CS1525 errors
            // Original line: int giaHienTai = sanPham.GiaKhuyenMai ? sanPham.GiaBan;

            // Corrected line:
            int giaHienTai = sanPham.GiaKhuyenMai > 0 ? sanPham.GiaKhuyenMai : sanPham.GiaBan;

            var existingChiTiet = await _context.ChiTietGioHang.FindAsync(gioHang.Id, sanPhamId);
            if (existingChiTiet != null)
            {
                existingChiTiet.SoLuong += soLuong;
            }
            else
            {
                gioHang.ChiTietGioHangs.Add(new ChiTietGioHang
                {
                    GioHangId = gioHang.Id,
                    SanPhamId = sanPhamId,
                    SoLuong = soLuong,
                    DonGia = giaHienTai
                });
            }
            return await _context.SaveChangesAsync() > 0;
        }
        // Xóa sản phẩm khỏi giỏ hàng
        public async Task<bool> DeletedSanPhamAsync(string? sessionId,int? khachHangId,int sanPhamId)
        {
            var gioHang = await GetGioHangAsync(sessionId, khachHangId);
            if(gioHang == null)
                return false;
            
            var chiTiet = gioHang.ChiTietGioHangs
                                .FirstOrDefault(ct => ct.SanPhamId == sanPhamId && ct.GioHangId == gioHang.Id);
            if (chiTiet == null) 
                return false;

            //Xóa giỏ hàng ở UI
            //gioHang.ChiTietGioHangs.Remove(chiTiet);
            //Xóa giỏ hàng ở DB
            _context.ChiTietGioHang.Remove(chiTiet);
            return await _context.SaveChangesAsync() > 0;

        }
        // Merge giỏ hàng khi khách hàng đăng nhập
        public async Task MergeCartAsync(string? sessionId,int? khachHangId)
        {
            var sessionCart = await GetGioHangAsync(sessionId, null);
            var khachHangCart = await GetGioHangAsync(null,khachHangId);

            if(sessionCart == null || khachHangCart == null) return;
            
            foreach(var item in sessionCart.ChiTietGioHangs)
            {
                var existing = khachHangCart.ChiTietGioHangs
                                                .FirstOrDefault(ct => ct.SanPhamId == item.SanPhamId);
                if (existing != null)
                    existing.SoLuong += item.SoLuong;
                else
                    khachHangCart.ChiTietGioHangs.Add(item);
            }

            _context.GioHang.Remove(sessionCart);
            
            await _context.SaveChangesAsync();
        }
    }
}
