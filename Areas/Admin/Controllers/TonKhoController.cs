using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QLCHBanDienThoaiMoi.Data;
using QLCHBanDienThoaiMoi.Models;
using QLCHBanDienThoaiMoi.Services;

namespace QLCHBanDienThoaiMoi.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TonKhoController : Controller
    {
        private readonly TonKhoService _tonKhoService;

        public TonKhoController(TonKhoService tonKhoService)
        {
            _tonKhoService = tonKhoService;
        }

        // GET: Admin/TonKho
        public async Task<IActionResult> Index()
        {
            return View(await _tonKhoService.GetAllTonKhoAsync());
        }
    }
}
