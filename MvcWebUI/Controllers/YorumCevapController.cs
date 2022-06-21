using Business.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MvcWebUI.Models;
using System.Security.Claims;

namespace MvcWebUI.Controllers
{
    public class YorumCevapController : Controller
    {
        private readonly IYorumCevapService _yorumCevapService;

        public YorumCevapController(IYorumCevapService yorumCevapService)
        {
            _yorumCevapService = yorumCevapService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult YorumCevapYaz(YorumViewModel model)
        {
            if (ModelState.IsValid)
            {
                var claimsIdentity = User.Identity as ClaimsIdentity;
                var userId = claimsIdentity.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Sid).Value;
                model.YorumCevap.OlusturanKullaniciId = Convert.ToInt32(userId);
                model.YorumCevap.KullaniciId = Convert.ToInt32(userId);
                var result = _yorumCevapService.Add(model.YorumCevap);
                TempData["Result"] = "success";
                TempData["Message"] = $"Cevap Eklendi.";
            }
            return RedirectToAction("Detay", "Yorum", new
            {
                id = model.YorumCevap.YorumId,
            });
        }

        [Authorize(Roles = "admin")]
        public IActionResult SoftDelete(int? id1,int? id2)
        {
            var result = _yorumCevapService.SoftDelete(id1.Value);
            if (result.IsSuccessful)
            {
                TempData["Result"] = "success";
                TempData["Message"] = "İşlem Başarılı";
                return RedirectToAction("Detay", "Yorum", new
                {
                    id = id2.Value
                });
            }
            else
            {
                TempData["Result"] = "danger";
                TempData["Message"] = $"İşlem Başarısız {result.Message}";
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index", "Home" );
        }
    }
}
