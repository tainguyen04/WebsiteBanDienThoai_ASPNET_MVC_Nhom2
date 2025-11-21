using QLCHBanDienThoaiMoi.DTO;
using QLCHBanDienThoaiMoi.Models;
using System.Collections.Generic;

public interface IHoaDonNhapService
{
    // Lấy tất cả hóa đơn nhập
    IEnumerable<HoaDonNhapDTO> GetAll();

    // Lấy hóa đơn nhập theo Id
    HoaDonNhapDTO? GetById(int id);

    // Tạo hóa đơn nhập từ entity
    void Create(HoaDonNhap hoaDonNhap);

    // Tạo hóa đơn nhập từ DTO
    void CreateWithDetails(HoaDonNhapDTO dto);

    // Cập nhật hóa đơn nhập từ entity
    void Update(HoaDonNhap hoaDonNhap);

    // Cập nhật hóa đơn nhập từ DTO
    void UpdateWithDetails(HoaDonNhapDTO dto);

    // Xóa hóa đơn nhập theo Id
    void Delete(int id);
}
