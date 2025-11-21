
using Microsoft.EntityFrameworkCore;
using QLCHBanDienThoaiMoi.Models;

namespace QLCHBanDienThoaiMoi.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // DbSet cho tất cả entity
    public DbSet<ChiTietHoaDonBan> ChiTietHoaDonBan { get; set; }
    public DbSet<ChiTietHoaDonNhap> ChiTietHoaDonNhap { get; set; }
    public DbSet<ChiTietGioHang> ChiTietGioHang { get; set; }
    public DbSet<DanhMucSanPham> DanhMucSanPham { get; set; }
    public DbSet<GioHang> GioHang { get; set; }
    public DbSet<HoaDonNhap> HoaDonNhap { get; set; }
    public DbSet<HoaDonBan> HoaDonBan { get; set; }
    public DbSet<KhachHang> KhachHang { get; set; }
    public DbSet<NhaCungCap> NhaCungCap { get; set; }
    public DbSet<NhanVien> NhanVien { get; set; }
    public DbSet<PhieuBaoHanh> PhieuBaoHanh { get; set; }
    public DbSet<SanPham> SanPham { get; set; }
    public DbSet<TaiKhoan> TaiKhoan { get; set; }
    public DbSet<KhuyenMai> KhuyenMai { get; set; }



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder); // Quan trọng để cấu hình Identity

        // ChiTietHoaDonNhap
        modelBuilder.Entity<ChiTietHoaDonNhap>()
            .HasKey(ct => new { ct.HoaDonNhapId, ct.SanPhamId });
        modelBuilder.Entity<ChiTietHoaDonNhap>()
            .HasOne(ct => ct.HoaDonNhap)
            .WithMany(h => h.ChiTietHoaDonNhaps)
            .HasForeignKey(ct => ct.HoaDonNhapId);
        modelBuilder.Entity<ChiTietHoaDonNhap>()
            .HasOne(ct => ct.SanPham)
            .WithMany(s => s.ChiTietHoaDonNhaps)
            .HasForeignKey(ct => ct.SanPhamId);

        // ChiTietHoaDonBan
        modelBuilder.Entity<ChiTietHoaDonBan>()
            .HasKey(ct => new { ct.HoaDonBanId, ct.SanPhamId });
        modelBuilder.Entity<ChiTietHoaDonBan>()
            .HasOne(ct => ct.HoaDonBan)
            .WithMany(h => h.ChiTietHoaDonBans)
            .HasForeignKey(ct => ct.HoaDonBanId);
        modelBuilder.Entity<ChiTietHoaDonBan>()
            .HasOne(ct => ct.SanPham)
            .WithMany(s => s.ChiTietHoaDonBans)
            .HasForeignKey(ct => ct.SanPhamId);
       
        // ChiTietGioHang
        modelBuilder.Entity<ChiTietGioHang>()
            .HasKey(ct => new { ct.GioHangId, ct.SanPhamId });
        modelBuilder.Entity<ChiTietGioHang>()
            .HasOne(ct => ct.GioHang)
            .WithMany(g => g.ChiTietGioHangs)
            .HasForeignKey(ct => ct.GioHangId);
        modelBuilder.Entity<ChiTietGioHang>()
            .HasOne(ct => ct.SanPham)
            .WithMany(s => s.ChiTietGioHangs)
            .HasForeignKey(ct => ct.SanPhamId);

        // DanhMucSanPham
        modelBuilder.Entity<DanhMucSanPham>()
            .HasKey(d => d.Id);
        modelBuilder.Entity<DanhMucSanPham>()
            .HasMany(d => d.SanPhams)
            .WithOne(s => s.DanhMucSanPham)
            .HasForeignKey(s => s.DanhMucId);

        // GioHang
        modelBuilder.Entity<GioHang>()
            .HasKey(g => g.Id);
        modelBuilder.Entity<GioHang>()
            .HasOne(g => g.KhachHang)
            .WithMany(k => k.GioHangs)
            .HasForeignKey(g => g.KhachHangId)
            .IsRequired(false);
        modelBuilder.Entity<GioHang>()
            .HasMany(g => g.ChiTietGioHangs)
            .WithOne(ct => ct.GioHang)
            .HasForeignKey(ct => ct.GioHangId);

       
        // HoaDonNhap
        modelBuilder.Entity<HoaDonNhap>()
            .HasKey(h => h.Id);
        modelBuilder.Entity<HoaDonNhap>()
            .HasOne(h => h.NhaCungCap)
            .WithMany()
            .HasForeignKey(h => h.NhaCungCapId);
        modelBuilder.Entity<HoaDonNhap>()
            .HasMany(h => h.ChiTietHoaDonNhaps)
            .WithOne(ct => ct.HoaDonNhap)
            .HasForeignKey(ct => ct.HoaDonNhapId);

        // HoaDonBan
        modelBuilder.Entity<HoaDonBan>()
            .HasKey(h => h.Id);
        modelBuilder.Entity<HoaDonBan>()
            .HasOne(h => h.KhachHang)
            .WithMany(k => k.HoaDonBans)
            .HasForeignKey(h => h.KhachHangId);
        modelBuilder.Entity<HoaDonBan>()
            .HasOne(h => h.NhanVien)
            .WithMany(n => n.HoaDonBans)
            .HasForeignKey(h => h.NhanVienId)
            .IsRequired(false);
        modelBuilder.Entity<HoaDonBan>()
            .HasMany(h => h.ChiTietHoaDonBans)
            .WithOne(ct => ct.HoaDonBan)
            .HasForeignKey(ct => ct.HoaDonBanId);

        // KhachHang
        modelBuilder.Entity<KhachHang>()
            .HasKey(k => k.Id);
       

        // NhaCungCap
        modelBuilder.Entity<NhaCungCap>()
            .HasKey(n => n.Id);
        modelBuilder.Entity<NhaCungCap>()
            .HasMany(n => n.HoaDonNhaps)
            .WithOne(h => h.NhaCungCap)
            .HasForeignKey(h => h.NhaCungCapId);

        // NhanVien
        modelBuilder.Entity<NhanVien>()
            .HasKey(n => n.Id);
        

        // PhieuBaoHanh
        modelBuilder.Entity<PhieuBaoHanh>()
            .HasKey(p => new {p.HoaDonBanId,p.SanPhamId});
        modelBuilder.Entity<PhieuBaoHanh>()
            .HasOne(p => p.ChiTietHoaDonBan)
            .WithOne(ct => ct.PhieuBaoHanh)
            .HasForeignKey<PhieuBaoHanh>(p => new { p.HoaDonBanId, p.SanPhamId })
            .IsRequired(true)
            .OnDelete(DeleteBehavior.NoAction);

        // SanPham
        modelBuilder.Entity<SanPham>()
            .HasKey(s => s.Id);
        modelBuilder.Entity<SanPham>()
            .HasOne(s => s.DanhMucSanPham)
            .WithMany(d => d.SanPhams)
            .HasForeignKey(s => s.DanhMucId);
        modelBuilder.Entity<SanPham>()
            .HasMany(s => s.ChiTietHoaDonNhaps)
            .WithOne(ct => ct.SanPham)
            .HasForeignKey(ct => ct.SanPhamId);
        modelBuilder.Entity<SanPham>()
            .HasMany(s => s.ChiTietHoaDonBans)
            .WithOne(ct => ct.SanPham)
            .HasForeignKey(ct => ct.SanPhamId);
        modelBuilder.Entity<SanPham>()
            .HasMany(s => s.ChiTietGioHangs)
            .WithOne(g => g.SanPham)
            .HasForeignKey(g => g.SanPhamId);
        modelBuilder.Entity<SanPham>()
            .HasOne(s => s.KhuyenMai)
            .WithMany(km => km.SanPhams)
            .HasForeignKey(s => s.KhuyenMaiId)
            .OnDelete(DeleteBehavior.SetNull); // Nếu xóa KM, sản phẩm vẫn giữ nguyên





        // TaiKhoan
        modelBuilder.Entity<TaiKhoan>()
        .HasKey(t => t.Id);
        modelBuilder.Entity<TaiKhoan>()
            .HasOne(t => t.KhachHang)
            .WithOne(k => k.TaiKhoan)
            .HasForeignKey<KhachHang>(k => k.Id)
            .IsRequired(false);
        modelBuilder.Entity<TaiKhoan>()
            .HasOne(t => t.NhanVien)
            .WithOne(n => n.TaiKhoan)
            .HasForeignKey<NhanVien>(n => n.Id)
            .IsRequired(false);

        // 🔹 KhuyenMai
        modelBuilder.Entity<KhuyenMai>()
            .HasKey(k => k.Id);
        modelBuilder.Entity<KhuyenMai>()
            .Property(k => k.GiaTri)
            .HasPrecision(18, 2);




    }

public DbSet<QLCHBanDienThoaiMoi.Models.TonKho> TonKho { get; set; } = default!;
}

