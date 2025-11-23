using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using QLCHBanDienThoaiMoi.Models;
using QLCHBanDienThoaiMoi.Services.Interfaces;

namespace QLCHBanDienThoaiMoi.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PhieuBaoHanhController : Controller
    {
        private readonly IPhieuBaoHanhService _phieuBaoHanhService;

        public PhieuBaoHanhController(IPhieuBaoHanhService phieuBaoHanhService)
        {
            _phieuBaoHanhService = phieuBaoHanhService;
        }

        // GET: PhieuBaoHanh
        public async Task<IActionResult> Index()
        {
            var list = await _phieuBaoHanhService.GetAllAsync();
            return View(list);
        }

        // GET: PhieuBaoHanh/Details
        public async Task<IActionResult> Details(int hoaDonBanId, int sanPhamId)
        {
            var phieu = await _phieuBaoHanhService.GetByIdAsync(hoaDonBanId, sanPhamId);
            if (phieu == null)
            {
                return NotFound();
            }
            return View(phieu);
        }

        // GET: PhieuBaoHanh/Create
        public async Task<IActionResult> Create()
        {
            await LoadDropDownData();
            return View();
        }

        // POST: PhieuBaoHanh/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("HoaDonBanId,SanPhamId,NgayLap,NgayHetHan,MoTa,TrangThai")] PhieuBaoHanh phieu)
        {
            if (ModelState.IsValid)
            {
                var result = await _phieuBaoHanhService.CreateAsync(phieu);

                if (result)
                {
                    TempData["SuccessMessage"] = "Thêm phiếu bảo hành thành công!";
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError("", "Không thể tạo phiếu bảo hành.");
            }

            await LoadDropDownData(phieu);
            return View(phieu);
        }

        // GET: PhieuBaoHanh/Edit
        public async Task<IActionResult> Edit(int hoaDonBanId, int sanPhamId)
        {
            var phieu = await _phieuBaoHanhService.GetByIdAsync(hoaDonBanId, sanPhamId);
            if (phieu == null)
                return NotFound();

            await LoadDropDownData(phieu);
            return View(phieu);
        }

        // POST: PhieuBaoHanh/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("HoaDonBanId,SanPhamId,NgayLap,NgayHetHan,MoTa,TrangThai")] PhieuBaoHanh phieu)
        {
            if (ModelState.IsValid)
            {
                var result = await _phieuBaoHanhService.UpdateAsync(phieu);

                if (result)
                {
                    TempData["SuccessMessage"] = "Cập nhật phiếu bảo hành thành công!";
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError("", "Không thể cập nhật phiếu bảo hành.");
            }

            await LoadDropDownData(phieu);
            return View(phieu);
        }

        // POST: PhieuBaoHanh/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int hoaDonBanId, int sanPhamId)
        {
            var result = await _phieuBaoHanhService.DeleteAsync(hoaDonBanId, sanPhamId);

            if (!result)
                return NotFound();

            TempData["SuccessMessage"] = "Xóa phiếu bảo hành thành công!";
            return RedirectToAction(nameof(Index));
        }

        // ======================
        // Load dropdown data
        // ======================
        private async Task LoadDropDownData(PhieuBaoHanh? phieu = null)
        {
            var hoaDonList = await _phieuBaoHanhService.GetHoaDonBanListAsync();
            var sanPhamList = await _phieuBaoHanhService.GetSanPhamListAsync();

            ViewData["HoaDonBanId"] = new SelectList(hoaDonList, "Id", "Id", phieu?.HoaDonBanId);
            ViewData["SanPhamId"] = new SelectList(sanPhamList, "Id", "TenSanPham", phieu?.SanPhamId);
        }
    }
}
