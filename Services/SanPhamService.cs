using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLCHBanDienThoaiMoi.Data;
using QLCHBanDienThoaiMoi.DTO;
using QLCHBanDienThoaiMoi.Models;
using QLCHBanDienThoaiMoi.Services.Interfaces;
using SlugGenerator;

namespace QLCHBanDienThoaiMoi.Services
{
    public class SanPhamService : ISanPhamService
    {
        private readonly ApplicationDbContext _context;
        private readonly IDanhMucSanPhamService _danhMucSanPhamService;
        public SanPhamService(ApplicationDbContext context, IDanhMucSanPhamService danhMucSanPhamService)
        {
            _context = context;
            _danhMucSanPhamService = danhMucSanPhamService;
        }
        // Updated method to fix CS8602: Dereference of a possibly null reference.
        public async Task<List<SanPham>> GetSanPhamsAsync()
        {
            var sanPhams = await _context.SanPham
                                         .Include(s => s.DanhMucSanPham)
                                         .Include(s => s.KhuyenMai)
                                         .AsNoTracking()
                                         .ToListAsync();
            return sanPhams;
        }
        public async Task<List<SanPhamDTO>> GetSanPhamHomePageAsync()
        {
            var sp = await _context.SanPham
                                       .Include(s => s.KhuyenMai)
                                       .AsNoTracking()
                                       .ToListAsync();
            return  sp.Select( s=> new SanPhamDTO
                                 {
                                     Id = s.Id,
                                     TenSanPham = s.TenSanPham,
                                     GiaBan = s.GiaBan,
                                     HangSanXuat = s.HangSanXuat,
                                     MoTa = s.MoTa,
                                     HinhAnh = s.HinhAnh,
                                     TenKhuyenMai = s.KhuyenMai != null ? s.KhuyenMai.TenKhuyenMai : string.Empty,
                                     GiaKhuyenMai = s.KhuyenMai != null ? (decimal) s.GiaBan * (1 - s.KhuyenMai.GiaTri/100) : s.GiaBan,
                                     PhanTramKhuyenMai = s.KhuyenMai != null ? s.KhuyenMai.GiaTri : 0
                                 }).ToList();
        }
        //Tạo mới sản phẩm
        public async Task<bool> CreateSanPhamAsync(IFormFile file, SanPham sanPham)
        {
            if (file != null && file.Length > 0)
            {
                sanPham.HinhAnh = await UploadFileAsync(file);
            }
            _context.SanPham.Add(sanPham);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
        //Lấy sản phẩm theo Id
        public async Task<SanPham?> GetSanPhamByIdAsync(int? id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            var sanPham = await _context.SanPham
                .Include(s => s.DanhMucSanPham)
                .Include(s => s.KhuyenMai)
                .FirstOrDefaultAsync(m => m.Id == id);

            return sanPham;
        }
        //Cập nhật sản phẩm
        public async Task<bool> UpdateSanPhamAsync(IFormFile? file, SanPham sanPham)
        {
            var existingSanPham = await _context.SanPham.FindAsync(sanPham.Id);
            if(existingSanPham == null)
            {
                return false;
            }
            // Cập nhật các thuộc tính trước khi xử lý ảnh
            _context.Entry(existingSanPham).CurrentValues.SetValues(sanPham);

            if (file != null && file.Length > 0)
            {
                if(!string.IsNullOrEmpty(existingSanPham.HinhAnh) && existingSanPham.HinhAnh != "default.png")
                {
                    var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", existingSanPham.HinhAnh);
                    if (File.Exists(oldPath))
                    {
                        File.Delete(oldPath);
                    }
                }
                var newFileName = await UploadFileAsync(file);
                existingSanPham.HinhAnh = newFileName;
            }
            sanPham.HinhAnh = existingSanPham.HinhAnh;
            // Cập nhật các thuộc tính trước khi xử lý ảnh
            _context.Entry(existingSanPham).CurrentValues.SetValues(sanPham);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
        //Xóa sản phẩm theo Id
        public async Task<SanPham?> DeleteByIdAsync(int? id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }
            var sanPham = await _context.SanPham.FindAsync(id);
            if (sanPham != null)
            {
                if(!string.IsNullOrEmpty(sanPham.HinhAnh) && sanPham.HinhAnh != "default.png")
                {
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", sanPham.HinhAnh);
                    if (File.Exists(path))
                    {
                        File.Delete(path);
                    }
                }
                _context.SanPham.Remove(sanPham);
                await _context.SaveChangesAsync();
            }
            return sanPham;
        }
        //Lấy tất cả danh mục sản phẩm
        public async Task<List<DanhMucSanPham>> GetAllDanhMucAsync()
        {
            return await _danhMucSanPhamService.GetAllDanhMucSanPhamAsync();
        }
        //Lấy tất cả khuyến mãi
        public async Task<List<KhuyenMaiDTO>> GetAllKhuyenMaiAsync()
        {
            return await _context.KhuyenMai
                                 .AsNoTracking()
                                 .Select(km => new KhuyenMaiDTO
                                 {
                                     Id = km.Id,
                                     TenKhuyenMai = km.TenKhuyenMai,
                                     GiaTri = km.GiaTri
                                 })
                                 .ToListAsync();
        }
        //Xử lý upload file
        public async Task<string> UploadFileAsync(IFormFile file)
        {

            if (file == null || file.Length == 0)
                return "default.png";
            var allowed = new HashSet<string> { ".jpg", ".png", ".jpeg" };
            //Lấy tên file trước dấu .
            var f = Path.GetFileNameWithoutExtension(file.FileName);
            //Chuẩn hóa tiếng việt thành không dấu
            f = f.GenerateSlug().Replace("-", "");
            //Lấy phần mở rộng sau dấu .
            var ext = Path.GetExtension(file.FileName).ToLower();

            if (!allowed.Contains(ext))
            {
                throw new Exception("Định dạng file không được phép!");
            }

            var fileName = $"{f}{ext}";
            var path = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot", "images", fileName);

            if (File.Exists(path))
            {
                throw new Exception("File đã tồn tại!");
            }

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return fileName;
        }
        public SanPhamService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SanPham>> SearchAsync(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return await _context.SanPham.ToListAsync();

            keyword = keyword.Trim().ToLower();

            return await _context.SanPham
                .Include(x => x.DanhMucSanPham)
                .Include(x => x.KhuyenMai)
                .Where(x =>
                    x.TenSanPham.ToLower().Contains(keyword) ||
                    x.HangSanXuat.ToLower().Contains(keyword) ||
                    x.DanhMucSanPham.TenDanhMuc.ToLower().Contains(keyword)
                )
                .ToListAsync();
        }
    }
}
