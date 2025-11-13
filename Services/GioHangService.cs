using Microsoft.EntityFrameworkCore;
using QLCHBanDienThoaiMoi.Data;
using QLCHBanDienThoaiMoi.DTO;
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
        public async Task<GioHang?> GetGioHangEntityAsync(string? sessionId, int? khachHangId)
        {
            var query = _context.GioHang.AsQueryable();
            if (khachHangId.HasValue)
            {
                query = query.Where(g => g.KhachHangId == khachHangId.Value);
            }
            else if (!string.IsNullOrEmpty(sessionId))
            {
                query = query.Where(g => g.SessionId == sessionId);
            }
            else
            {
                return null;
            }
            return await query.FirstOrDefaultAsync();
        }
        // Lấy chi tiết giỏ hàng theo sessionId hoặc khachHangId
        public async Task<List<ChiTietGioHangDTO>> GetGioHangAsync(string? sessionId, int? khachHangId)
        {
            var gh = _context.ChiTietGioHang
                            .Include(g => g.GioHang)
                            .Include(g => g.SanPham)
                                .ThenInclude(g => g.KhuyenMai)
                            .AsQueryable();

            if (khachHangId.HasValue)
            {
                gh = gh.Where(g => g.GioHang.KhachHangId == khachHangId.Value);
            }
            else if(!string.IsNullOrEmpty(sessionId))
            {
                gh = gh.Where(g => g.GioHang.SessionId == sessionId);
            }
            else
            {
                return new List<ChiTietGioHangDTO>();
            }

            return await gh.Select(gh => new ChiTietGioHangDTO
                {
                    SessionId = gh.GioHang.SessionId,
                    KhachHangId = gh.GioHang.KhachHangId,
                    SanPhamId = gh.SanPhamId,
                    HinhAnh = gh.SanPham.HinhAnh,
                    TenSanPham = gh.SanPham.TenSanPham,
                    SoLuong = gh.SoLuong,
                    GiaBan = gh.SanPham.GiaBan,
                    GiaKhuyenMai = gh.SanPham.KhuyenMai != null && gh.SanPham.KhuyenMai.GiaTri > 0
                                            ? gh.SanPham.GiaBan * (1 - gh.SanPham.KhuyenMai.GiaTri / 100)
                                            : gh.SanPham.GiaBan
            }).ToListAsync();

        }
        // Tạo giỏ hàng mới
        public async Task<bool> CreateGioHangAsync(string? sessionId, int? khachHangId)
        {
            if(string.IsNullOrEmpty(sessionId) && !khachHangId.HasValue)
            {
                throw new ArgumentException("Phải có sessionId hoặc khachhangId");
            }
            var existingGioHang = await GetGioHangEntityAsync(sessionId, khachHangId);
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
        public async Task<bool> DeletedSanPhamFromGioHangAsync(string? sessionId,int? khachHangId,int sanPhamId)
        {
            var query =  _context.ChiTietGioHang.Include(ct => ct.GioHang).AsQueryable();
            if(!string.IsNullOrEmpty(sessionId))
            {
                query = query.Where(ct => ct.GioHang.SessionId == sessionId);
            }
            else if(khachHangId.HasValue)
            {
                query = query.Where(ct => ct.GioHang.KhachHangId == khachHangId.Value);
            }
            else
            {
                return false;
            }
            var chiTiet = await query.FirstOrDefaultAsync(ct => ct.SanPhamId == sanPhamId);
            if (chiTiet == null)
                return false;

            
            //Xóa giỏ hàng ở DB
            _context.ChiTietGioHang.Remove(chiTiet);
            return await _context.SaveChangesAsync() > 0;

        }
        // Merge giỏ hàng khi khách hàng đăng nhập
        public async Task<bool> MergeCartAsync(string? sessionId,int? khachHangId)
        {
            var sessionCart =  await GetGioHangEntityAsync(sessionId, null);
            var khachHangCart =  await GetGioHangEntityAsync(null, khachHangId);

            if (sessionCart == null || khachHangCart == null) return false;
           

            var mergeCart = await _context.ChiTietGioHang
                                          .Where(ct => ct.GioHangId == sessionCart.Id)
                                          .ExecuteUpdateAsync(ct => ct
                                              .SetProperty(c => c.GioHangId, khachHangCart.Id));

            _context.GioHang.Remove(sessionCart);
            
            await _context.SaveChangesAsync();
            return mergeCart > 0;
        }
    }
}
