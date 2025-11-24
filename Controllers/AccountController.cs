using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using QLCHBanDienThoaiMoi.Models;
using QLCHBanDienThoaiMoi.Services.Interfaces;
using QLCHBanDienThoaiMoi.Helpers;

namespace QLCHBanDienThoaiMoi.Controllers
{
    public class AccountController : Controller
    {
        private readonly ITaiKhoanService _taiKhoanService;
        private readonly SessionHelper _sessionHelper;

        public AccountController(ITaiKhoanService taiKhoanService, SessionHelper sessionHelper)
        {
            _taiKhoanService = taiKhoanService;
            _sessionHelper = sessionHelper;
        }

        // GET: /Account/Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        // POST: /Account/Login
        [HttpPost]
        // POST: /Account/Login
[HttpPost]
        // POST: /Account/Login
        [HttpPost]
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                ViewBag.Error = "Vui lòng nhập tên đăng nhập và mật khẩu!";
                return View();
            }

            var user = await _taiKhoanService.DangNhap(username, password);

            if (user == null)
            {
                ViewBag.Error = "Tài khoản không tồn tại, sai mật khẩu hoặc đã bị khóa!";
                return View();
            }

            // === TẠO ROLE NAME CHO PHÂN QUYỀN ===
            string roleName = user.VaiTro switch
            {
                VaiTro.Admin => "Admin",
                VaiTro.Staff => "Staff",
                _ => "User"
            };

            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, user.TenDangNhap),
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Role, roleName),
        new Claim("UserId", user.Id.ToString())
    };

            // Thêm Id nếu có
            if (user.KhachHang != null)
                claims.Add(new Claim("KhachHangId", user.KhachHang.Id.ToString()));
            if (user.NhanVien != null)
                claims.Add(new Claim("NhanVienId", user.NhanVien.Id.ToString()));

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            var authProps = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(30)
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProps);

            // === ĐIỀU HƯỚNG THEO VAI TRÒ ===
            string redirectUrl = user.VaiTro switch
            {
                VaiTro.Admin => "/Admin/",           // Vào Area Admin
                VaiTro.Staff => "/Admin/",           // Tạm thời cho Staff vào chung Admin
                VaiTro.User => "/",                          // Trang chủ khách hàng
                _ => "/"
            };

            TempData["ThongBao"] = $"Chào mừng {user.TenDangNhap}! ({roleName})";
            return Redirect(redirectUrl);
        }

        // GET: /Account/Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        public async Task<IActionResult> Register(
            string username,
            string password,
            string confirmPassword,
            string tenKH,
            string diachi,
            string sdt,
            string email)
        {
            if (string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(confirmPassword))
            {
                ViewBag.Error = "Vui lòng nhập đầy đủ thông tin bắt buộc!";
                return View();
            }

            if (password != confirmPassword)
            {
                ViewBag.Error = "Mật khẩu xác nhận không khớp!";
                return View();
            }

            if (await _taiKhoanService.KiemTraTenDangNhap(username))
            {
                ViewBag.Error = "Tên đăng nhập đã tồn tại!";
                return View();
            }

            var taiKhoan = new TaiKhoan
            {
                TenDangNhap = username,
                MatKhau = password, // Service sẽ tự hash
                VaiTro = VaiTro.User,
                TrangThai = TrangThaiTaiKhoan.Active
            };

            var khachHang = new KhachHang
            {
                TenKhachHang = tenKH,
                DiaChi = diachi,
                SoDienThoai = sdt,
                Email = email
            };

            var sessionId = _sessionHelper.EnsureSessionIdExists();
            bool success = await _taiKhoanService.DangKyAsync(taiKhoan, khachHang, sessionId);

            if (!success)
            {
                ViewBag.Error = "Đăng ký thất bại. Vui lòng thử lại!";
                return View();
            }

            TempData["ThongBao"] = "Đăng ký thành công! Bạn có thể đăng nhập ngay.";
            return RedirectToAction("Login");
        }

        // Đăng xuất
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            TempData["ThongBao"] = "Bạn đã đăng xuất thành công!";
            return RedirectToAction("Login");
        }
    }
}