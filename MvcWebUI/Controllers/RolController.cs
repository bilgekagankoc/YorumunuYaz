using Business.Models;
using Business.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MvcWebUI.Controllers
{
    [Authorize(Roles = "admin")]
    public class RolController : Controller
    {
        private readonly IRolService _rolService;
        private readonly IKullaniciService _kullaniciService;
        public RolController(IRolService rolService,IKullaniciService kullaniciService)
        {
            _kullaniciService = kullaniciService;
            _rolService = rolService;
        }

        public IActionResult Index()
        {
            var result = _rolService.Query().ToList();
            return View(result);
        }

        public IActionResult Kayit()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Kayit(RolModel model)
        {
            if (ModelState.IsValid)
            {
                var claimsIdentity = User.Identity as ClaimsIdentity;
                var userId = claimsIdentity.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Sid).Value;
                model.OlusturanKullaniciId = Convert.ToInt32(userId);
                var result = _rolService.Add(model);
                if (result.IsSuccessful)
                {
                    TempData["Result"] = "success";
                    TempData["Message"] = "İşlem Başarılı";
                    return RedirectToAction("Index", "Rol");
                }
                else
                {
                    TempData["Result"] = "danger";
                    TempData["Message"] = $"İşlem Başarısız {result.Message}";
                    return View(model);
                }
            }
            else
            {
                TempData["Result"] = "danger";
                TempData["Message"] = "İşlem Başarısız";
                return RedirectToAction("Index", "Rol");
            }
        }

        public IActionResult Sil(int? id)
        {
            var result = _rolService.Delete(id.Value);
            if (result.IsSuccessful)
            {
                TempData["Result"] = "success";
                TempData["Message"] = "Rol Silme İşlemi Başarılı";
                return RedirectToAction("Index", "Rol");
            }
            else
            {
                TempData["Result"] = "danger";
                TempData["Message"] = $"Rol Silme İşlemi Başarısız {result.Message}";
                return RedirectToAction("Index", "Rol");
            }
        }

        public IActionResult Duzenle(int? id)
        {   
            if(id == null)
            {
                TempData["Result"] = "danger";
                TempData["Message"] = "Rol Bulunamadı Id Gereklidir";
                return RedirectToAction("Index", "Rol");
            }
            var result = _rolService.Query().SingleOrDefault(x => x.Id == id);
            if (result == null)
            {
                TempData["Result"] = "danger";
                TempData["Message"] = "Rol Bulunamadı";
                return RedirectToAction("Index", "Rol");
            }
            else
            {
                return View(result);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Duzenle(RolModel model)
        {
            if (ModelState.IsValid)
            {
                var claimsIdentity = User.Identity as ClaimsIdentity;
                var userId = claimsIdentity.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Sid).Value;
                model.GuncelleyenKullaniciId = Convert.ToInt32(userId);
                var result = _rolService.Update(model);
                if (result.IsSuccessful)
                {
                    TempData["Result"] = "success";
                    TempData["Message"] = "Rol Duzenleme İşlemi Başarılı";
                    return RedirectToAction("Index", "Rol");
                }
                else
                {
                    TempData["Result"] = "danger";
                    TempData["Message"] = $"Rol Duzenleme İşlemi Başarısız {result.Message}";
                    return RedirectToAction("Index", "Rol");
                }
            }
            else
            {
                return View(model);
            }
        }

        public IActionResult Detay(int? id)
        {
            if(id == null)
            {
                TempData["Result"] = "danger";
                TempData["Message"] = "Detay işlemi başarısız id gereklidir";
                return RedirectToAction("Index", "Rol");
            }
            var result = _rolService.RolGetir(id.Value);
            if(result.IsSuccessful)
            {
                result.Data.OlusturanKullaniciAdi = _kullaniciService.Query().FirstOrDefault(x => x.Id == result.Data.OlusturanKullaniciId).KullaniciAdi;
                result.Data.GuncelleyenKullaniciAdi = _kullaniciService.Query().Any(x=>x.Id == result.Data.GuncelleyenKullaniciId) ? _kullaniciService.Query().FirstOrDefault(x=>x.Id == result.Data.GuncelleyenKullaniciId).KullaniciAdi : "Güncelleme Yapılmadı ";
                return View(result.Data);
            }
            else
            {
                TempData["Result"] = "danger";
                TempData["Message"] = $"Detay işlemi başarısız rol bulunamadı. {result.Message}";
                return RedirectToAction("Index", "Rol");
            }
        }


    }
}
