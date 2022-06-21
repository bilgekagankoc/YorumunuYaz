using Business.Services;
using Microsoft.AspNetCore.Mvc;
using MvcWebUI.Models;
using System.Diagnostics;

namespace MvcWebUI.Controllers
{
    public class HomeController : Controller
    {

        private readonly ILogger<HomeController> _logger;
        private readonly IYorumService _yorumService;
        private readonly IYorumCevapService _yorumCevapService;
        private readonly IKategoriService _kategoriService;

        public HomeController(ILogger<HomeController> logger, IYorumService yorumService,IKategoriService kategoriService, IYorumCevapService yorumCevapService)
        {
            _logger = logger;
            _yorumService = yorumService;
            _yorumCevapService = yorumCevapService;
            _kategoriService = kategoriService;
        }

        public IActionResult Index()
        {
            var result = _yorumService.Query().Where(x => x.AktifMi).ToList();
            YorumViewModel yvm = new YorumViewModel()
            {
                YorumModels = result,
                MevcutKategori = "Tüm Yorumlar"
            };
            return View(yvm);
        }


        public IActionResult Kategori(int? id)
        {
            var result = _yorumService.Query().Where(x => x.AktifMi && x.KategoriId == id).ToList();
            var kategori = _kategoriService.Query().FirstOrDefault(x => x.Id == id);
            YorumViewModel yvm = new YorumViewModel()
            {
                YorumModels = result,
                MevcutKategori = kategori.Ad
            };
            return View("Index",yvm);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}