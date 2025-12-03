# 🛒 Website Bán Điện thoại – ASP.NET Core MVC

## 📌 1. Tổng quan dự án
Dự án là một website bán điện thoại được xây dựng bằng ASP.NET Core MVC và Entity Framework Core với các chức năng chính:

- Xem danh sách và chi tiết sản phẩm  
- Giỏ hàng dùng Session + User (tự động đồng bộ khi đăng nhập)  
- Áp dụng khuyến mãi theo phần trăm  
- Load thêm sản phẩm bằng AJAX (Load More)  
- Quản trị sản phẩm, danh mục, khuyến mãi, tài khoản  
- Hệ thống phân quyền cơ bản: User – Admin  
- Tự động tạo hoặc xóa KhachHang/NhanVien khi thay đổi VaiTro  

Mục tiêu: xây dựng website thương mại điện tử mẫu với đầy đủ thao tác CRUD cơ bản.

## 🏗 2. Kiến trúc dự án
Dự án được xây dựng theo mô hình MVC kết hợp Service Layer, tách biệt rõ ràng các phần giao diện, nghiệp vụ và dữ liệu.
Toàn bộ cấu trúc thư mục, controller, services và ViewModels đã được tổ chức đầy đủ trong repo.

Tính chất kỹ thuật nổi bật:
- Entity Framework Core (Code First, LINQ, Include, AsNoTracking)
- Service Layer tách biệt nghiệp vụ
- DTO/ViewModel để truyền dữ liệu sang View
- Dependency Injection toàn hệ thống
- Giỏ hàng hỗ trợ 2 chế độ: SessionId + KhachHangId
- Đồng bộ giỏ hàng khi đăng nhập
- AJAX Load More không reload trang
- Tự động cập nhật KhachHang/NhanVien khi thay đổi VaiTro

## ⚙️ 3. Cách cài đặt & chạy dự án

### 1️⃣ Clone dự án
```bash
git clone https://github.com/<your-repo>
```

### 2️⃣ Cấu hình kết nối SQL Server
Trong file `appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=.;Database=QLCHBanDienThoaiMoi;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

### 3️⃣ Khởi tạo database
```bash
Update-Database
```

### 4️⃣ Chạy ứng dụng
```bash
dotnet run
```
Hoặc chạy bằng Visual Studio (F5).

