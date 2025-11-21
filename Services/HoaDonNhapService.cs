using Microsoft.EntityFrameworkCore;
using QLCHBanDienThoaiMoi.Data;
using QLCHBanDienThoaiMoi.DTO;
using QLCHBanDienThoaiMoi.Models;

public class HoaDonNhapService : IHoaDonNhapService
{
    private readonly ApplicationDbContext _context;

    public HoaDonNhapService(ApplicationDbContext context)
    {
        _context = context;
    }

    // Lấy tất cả hóa đơn nhập (DTO)
    public IEnumerable<HoaDonNhapDTO> GetAll()
    {
        return _context.HoaDonNhap
            .Include(h => h.NhaCungCap)
            .Include(h => h.ChiTietHoaDonNhaps)
            .ThenInclude(c => c.SanPham)
            .OrderByDescending(h => h.NgayLap)
            .Select(h => new HoaDonNhapDTO
            {
                Id = h.Id,
                NgayLap = h.NgayLap,
                NhaCungCapId = h.NhaCungCapId,
                NhaCungCapName = h.NhaCungCap != null ? h.NhaCungCap.TenNCC : "",
                TongTien = h.TongTien,
                ChiTietSanPhams = h.ChiTietHoaDonNhaps.Select(c => new ChiTietHoaDonNhapDTO
                {
                    SanPhamId = c.SanPhamId,
                    TenSanPham = c.SanPham != null ? c.SanPham.TenSanPham : "",
                    SoLuong = c.SoLuong,
                    GiaNhap = c.GiaNhap
                }).ToList()
            })
            .ToList();
    }

    // Lấy hóa đơn nhập theo Id (DTO)
    public HoaDonNhapDTO? GetById(int id)
    {
        var h = _context.HoaDonNhap
            .Include(hd => hd.NhaCungCap)
            .Include(hd => hd.ChiTietHoaDonNhaps)
            .ThenInclude(c => c.SanPham)
            .FirstOrDefault(hd => hd.Id == id);

        if (h == null) return null;

        return new HoaDonNhapDTO
        {
            Id = h.Id,
            NgayLap = h.NgayLap,
            NhaCungCapId = h.NhaCungCapId,
            NhaCungCapName = h.NhaCungCap?.TenNCC ?? "",
            TongTien = h.TongTien,
            ChiTietSanPhams = h.ChiTietHoaDonNhaps.Select(c => new ChiTietHoaDonNhapDTO
            {
                SanPhamId = c.SanPhamId,
                TenSanPham = c.SanPham?.TenSanPham ?? "",
                SoLuong = c.SoLuong,
                GiaNhap = c.GiaNhap
            }).ToList()
        };
    }

    // Tạo hóa đơn nhập từ entity
    public void Create(HoaDonNhap hoaDonNhap)
    {
        _context.HoaDonNhap.Add(hoaDonNhap);
        _context.SaveChanges();
    }

    // Tạo hóa đơn nhập từ DTO
    public void CreateWithDetails(HoaDonNhapDTO dto)
    {
        if (dto.ChiTietSanPhams == null || !dto.ChiTietSanPhams.Any())
            throw new Exception("Phiếu nhập phải có ít nhất một sản phẩm.");

        var hoaDon = new HoaDonNhap
        {
            NhaCungCapId = dto.NhaCungCapId,
            NgayLap = DateTime.Now,
            TongTien = dto.ChiTietSanPhams.Sum(c => c.SoLuong * c.GiaNhap)
        };

        _context.HoaDonNhap.Add(hoaDon);
        _context.SaveChanges(); // cần để có Id

        foreach (var ct in dto.ChiTietSanPhams)
        {
            var chiTiet = new ChiTietHoaDonNhap
            {
                HoaDonNhapId = hoaDon.Id,
                SanPhamId = ct.SanPhamId,
                SoLuong = ct.SoLuong,
                GiaNhap = ct.GiaNhap
            };
            _context.ChiTietHoaDonNhap.Add(chiTiet);

            // Cập nhật tồn kho
            var sp = _context.SanPham.Find(ct.SanPhamId);
            //if (sp != null) sp.SoLuongTon += ct.SoLuong;
        }

        _context.SaveChanges();
    }

    // Cập nhật hóa đơn nhập từ entity (triển khai interface)
    public void Update(HoaDonNhap hoaDonNhap)
    {
        _context.HoaDonNhap.Update(hoaDonNhap);
        _context.SaveChanges();
    }

    // Cập nhật hóa đơn nhập từ DTO
    public void UpdateWithDetails(HoaDonNhapDTO dto)
    {
        var hoaDon = _context.HoaDonNhap
            .Include(h => h.ChiTietHoaDonNhaps)
            .FirstOrDefault(h => h.Id == dto.Id);

        if (hoaDon == null) throw new Exception("Không tìm thấy hóa đơn");

        hoaDon.NhaCungCapId = dto.NhaCungCapId;
        hoaDon.NgayLap = dto.NgayLap;
        hoaDon.TongTien = dto.ChiTietSanPhams.Sum(c => c.SoLuong * c.GiaNhap);

        // Xóa chi tiết cũ
        _context.ChiTietHoaDonNhap.RemoveRange(hoaDon.ChiTietHoaDonNhaps);

        // Thêm chi tiết mới
        hoaDon.ChiTietHoaDonNhaps = dto.ChiTietSanPhams.Select(c => new ChiTietHoaDonNhap
        {
            HoaDonNhapId = hoaDon.Id,
            SanPhamId = c.SanPhamId,
            SoLuong = c.SoLuong,
            GiaNhap = c.GiaNhap
        }).ToList();

        _context.Update(hoaDon);
        _context.SaveChanges();
    }

    // Xóa hóa đơn nhập
    public void Delete(int id)
    {
        var hoaDon = _context.HoaDonNhap
            .Include(h => h.ChiTietHoaDonNhaps)
            .FirstOrDefault(h => h.Id == id);

        if (hoaDon != null)
        {
            _context.ChiTietHoaDonNhap.RemoveRange(hoaDon.ChiTietHoaDonNhaps);
            _context.HoaDonNhap.Remove(hoaDon);
            _context.SaveChanges();
        }
    }
}
