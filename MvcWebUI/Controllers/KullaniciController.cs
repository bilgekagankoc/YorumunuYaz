using Business.Models;
using Business.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MvcWebUI.Controllers
{
    [Authorize(Roles = "admin")]
    public class KullaniciController : Controller
    {
        private readonly IKullaniciService _kullaniciService;
        private readonly IRolService _rolService;

        public KullaniciController(IKullaniciService kullaniciService, IRolService rolService)
        {
            _kullaniciService = kullaniciService;
            _rolService = rolService;
        }

        public IActionResult Index()
        {
            var result = _kullaniciService.Query().ToList();
            return View(result);
        }
        public IActionResult Duzenle(int id)
        {
            var result = _kullaniciService.Query().FirstOrDefault(x => x.Id == id);
            if (result == null)
            {
                TempData["Result"] = "danger";
                TempData["Message"] = "Kullanıcı Bulunamadı";
                return RedirectToAction("Index", "Kullanicilar");
            }
            else
            {
                var roller = _rolService.RolleriGetir();
                ViewBag.RolId = new SelectList(roller.Data, "Id", "Adi", result.RolId.Value);
                return View(result);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Duzenle(KullaniciModel model)
        {
            if (model != null)
            {
                var result = _kullaniciService.Update(model);
                if (result.IsSuccessful)
                {
                    TempData["Result"] = "success";
                    TempData["Message"] = $"{model.KullaniciAdi} Başarıyla Güncellendi";
                    return RedirectToAction(nameof(Index));
                }
                TempData["Result"] = "danger";
                TempData["Message"] = "Kullanıcı Bulunamadı";
            }
            var roller = _rolService.RolleriGetir();
            ViewBag.RolId = new SelectList(roller.Data, "Id", "Adi", model.RolId.Value);
            return View(model);
        }
        // TODO detay yapılacak girdiği mesaj istatistikleri ile beraber -- sil yapılacak
    }
}
