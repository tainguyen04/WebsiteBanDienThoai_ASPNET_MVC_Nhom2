using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using QLCHBanDienThoaiMoi.Models;
using QLCHBanDienThoaiMoi.Services.Interfaces;
using QLCHBanDienThoaiMoi.Helpers;
using QLCHBanDienThoaiMoi.Services;

namespace QLCHBanDienThoaiMoi.Controllers
{
    public class AccountController : Controller
    {
        private readonly ITaiKhoanService _taiKhoanService;
        private readonly SessionHelper _sessionHelper;

        public AccountController(ITaiKhoanService taiKhoanService,SessionHelper sessionHelper)
        {
            _taiKhoanService = taiKhoanService;
            _sessionHelper = sessionHelper;
        }

        // ---------------------------------------------------
        // GET: Login
        // ---------------------------------------------------
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // ---------------------------------------------------
        // POST: Login
        // ---------------------------------------------------
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var user = await _taiKhoanService.DangNhap(username, password);

            if (user == null)
            {
                ViewBag.Error = "Tên đăng nhập hoặc mật khẩu không đúng!";
                return View();
            }

            // Tạo Claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.TenDangNhap),
                new Claim(ClaimTypes.Role, user.VaiTro.ToString()),
                new Claim("UserId", user.Id.ToString())
            };
            if(user.KhachHang != null)
            {
                claims.Add(new Claim("KhachHangId", user.KhachHang?.Id.ToString() ?? ""));
            }
            if (user.NhanVien != null)
            {
                claims.Add(new Claim("NhanVienId", user.NhanVien.Id.ToString()));
            }

            // Tạo danh tính
            var claimsIdentity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme
            );

            // Đăng nhập bằng Cookie Auth
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity)
            );

            TempData["ThongBao"] = "Đăng nhập thành công";

            return RedirectToAction("Index", "Home");
        }

        // ---------------------------------------------------
        // GET: Register
        // ---------------------------------------------------
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // ---------------------------------------------------
        // POST: Register
        // ---------------------------------------------------
        [HttpPost]
        public async Task<IActionResult> Register(
            string username,
            string password,
            string confirmPassword,
            string tenKH,
            string diachi,
            string sdt,
            string email
        )
        {
            var sessionId = _sessionHelper.EnsureSessionIdExists();
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

            var tk = new TaiKhoan
            {
                TenDangNhap = username,
                MatKhau = password,
                VaiTro = VaiTro.User
            };

            var kh = new KhachHang
            {
                TenKhachHang = tenKH,
                DiaChi = diachi,
                Email = email,
                SoDienThoai = sdt
            };

            bool success = await _taiKhoanService.DangKyAsync(tk, kh, sessionId);

            if (!success)
            {
                ViewBag.Error = "Không thể đăng ký. Vui lòng thử lại!";
                return View();
            }

            TempData["ThongBao"] = "Đăng ký thành công!";
            return RedirectToAction("Login");
        }
        // GET: KhachHangs/Edit/5
        public async Task<IActionResult> ChangePassword()
        {
            var userIdClaim = User.FindFirst("KhachHangId")?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                return RedirectToAction("Login", "Account");

            var taiKhoan = await _taiKhoanService.GetTaiKhoanByIdAsync(userId);
            if (taiKhoan == null)
            {
                return NotFound();
            }
            return View(taiKhoan);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(string oldPassword, string newPassword)
        {

            try
            {
                var userIdClaim = User.FindFirst("KhachHangId")?.Value;
                if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                    return RedirectToAction("Login","Account");

                var taiKhoan = await _taiKhoanService.ChangePasswordAsync(userId,oldPassword, newPassword);
                if (!taiKhoan)
                {
                    return RedirectToAction("ChangePassword", "Account");
                }
                else
                {
                    TempData["SuccessMessage"] = "Đổi mật khẩu thành công";
                    return RedirectToAction("Index", "Home");
                }
            }
            catch
            {
                return NotFound();
            }
        }

        // ---------------------------------------------------
        // Đăng xuất
        // ---------------------------------------------------
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login");
        }
    }
}
