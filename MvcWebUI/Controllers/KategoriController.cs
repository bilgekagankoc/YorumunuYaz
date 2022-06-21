using AppCore.Business.Models.Results;
using Business.Models;
using Business.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MvcWebUI.Controllers
{
    [Authorize(Roles = "admin")]
    public class KategoriController : Controller
    {
        private readonly IKategoriService _kategoriService;

        public KategoriController(IKategoriService kategoriService)
        {
            _kategoriService = kategoriService;
        }

        public IActionResult Index()
        {
            var model = _kategoriService.Query().ToList();
            return View(model);
        }

        public IActionResult Ekle()
        {
            return View();
        }

        public IActionResult Sil(int? id)
        {
            var result = _kategoriService.Delete(id.Value);
            if (result.IsSuccessful)
            {
                TempData["Result"] = "success";
                TempData["Message"] = "Kategori Silme İşlemi Başarılı";
                return RedirectToAction("Index", "Kategori");
            }
            else
            {
                TempData["Result"] = "danger";
                TempData["Message"] = $"Kategori Silme İşlemi Başarısız {result.Message}";
                return RedirectToAction("Index", "Kategori");
            }
        }

        public IActionResult Duzenle(int? id)
        {
            var model = _kategoriService.Query().FirstOrDefault(x => x.Id == id);
            if (model == null)
            {
                TempData["Result"] = "danger";
                TempData["Message"] = "Kategori Bulunamadı";
                return RedirectToAction("Index", "Kategori");
            }
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Duzenle(KategoriModel model)
        {
            if (ModelState.IsValid)
            {
                var claimsIdentity = User.Identity as ClaimsIdentity;
                var userId = claimsIdentity.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Sid).Value;
                model.GuncelleyenKullaniciId = Convert.ToInt32(userId);
                var result = _kategoriService.Update(model);
                if (result.IsSuccessful)
                {
                    TempData["Result"] = "success";
                    TempData["Message"] = "Kategori Duzenleme İşlemi Başarılı";
                    return RedirectToAction("Index", "Kategori");
                }
                else
                {
                    TempData["Result"] = "danger";
                    TempData["Message"] = $"Kategori Duzenleme İşlemi Başarısız {result.Message}";
                    return RedirectToAction("Index", "Kategori");
                }
            }
            else
            {
                return View(model);
            }
        }

        public IActionResult Detay(int? id)
        {
            var model = _kategoriService.Query().FirstOrDefault(x => x.Id == id);
            if (model == null)
            {
                TempData["Result"] = "danger";
                TempData["Message"] = "Kategori Bulunamadı";
                return RedirectToAction("Index", "Kategori");
            }
            return View(model);
        }
    }
}
