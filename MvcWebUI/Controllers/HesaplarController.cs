using Business.Models;
using Business.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace MvcWebUI.Controllers
{
    public class HesaplarController : Controller
    {
        private readonly IHesapService _hesapService;
        private readonly IRolService _rolService;

        public HesaplarController(IHesapService hesapService, IRolService rolService)
        {
            _hesapService = hesapService;
            _rolService = rolService;
        }
        public IActionResult Giris()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Giris(KullaniciGirisModel model)
        {
            if (ModelState.IsValid)
            {
                var result = _hesapService.Giris(model);
                if (result.IsSuccessful)
                {
                    List<Claim> claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name, result.Data.KullaniciAdi),
                        new Claim(ClaimTypes.Role, result.Data.RolAdiDisplay),
                        new Claim(ClaimTypes.Email, result.Data.ePosta),
                        new Claim(ClaimTypes.Sid, result.Data.Id.ToString())
                    };
                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", result.Message);
            }
            return View(model);
        }

        public IActionResult YetkisizIslem()
        {
            return View("Hata", "Bu işlem için yetkiniz bulunmamaktadır!");
        }

        public async Task<IActionResult> Cikis()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Kayit()
        {
            var roller = _rolService.RolleriGetir();
            ViewBag.RolId = new SelectList(roller.Data, "Id", "Adi");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Kayit(KullaniciKayitModel model)
        {
            if (ModelState.IsValid)
            {
                var result = _hesapService.Kayit(model);
                if (result.IsSuccessful)
                {
                    TempData["Result"] = "success";
                    TempData["Message"] = "İşlem Başarılı";
                    return RedirectToAction("Giris", "Hesaplar");
                }
                else
                {
                    TempData["Result"] = "danger";
                    TempData["Message"] = $"İşlem Başarısız {result.Message}";
                    return View(model);
                }

            }
            return View(model);
        }
    }
}
