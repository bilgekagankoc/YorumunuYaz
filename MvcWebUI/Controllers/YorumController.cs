using Business.Models;
using Business.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MvcWebUI.Models;
using MvcWebUI.Settings;
using System.Security.Claims;

namespace MvcWebUI.Controllers
{
    public class YorumController : Controller
    {
        private readonly IYorumService _yorumService;
        private readonly IYorumCevapService _yorumCevapService;

        public YorumController(IYorumService yorumService, IYorumCevapService yorumCevapService, IKategoriService kategoriService)
        {
            _yorumService = yorumService;
            _yorumCevapService = yorumCevapService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult YorumYaz(YorumViewModel model, IFormFile imaj)
        {
            if (ModelState.IsValid)
            {
                var claimsIdentity = User.Identity as ClaimsIdentity;
                var userId = claimsIdentity.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Sid).Value;
                model.YorumModel.OlusturanKullaniciId = Convert.ToInt32(userId);
                model.YorumModel.ImajDosyaUzantisi = imaj == null ? null : Path.GetExtension(imaj.FileName);
                model.YorumModel.Guid = Guid.NewGuid().ToString();
                var result = _yorumService.Add(model.YorumModel);
                if (result.IsSuccessful)
                {
                    bool? imajKaydetSonuc = ImajKaydet(model.YorumModel, imaj);
                    TempData["Result"] = "success";
                    TempData["Message"] = "İşlem Başarılı";
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    TempData["Result"] = "danger";
                    TempData["Message"] = $"İşlem Başarısız {result.Message}";
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                TempData["Result"] = "danger";
                TempData["Message"] = "İşlem Başarısız";
                return RedirectToAction("Index", "Home");
            }

        }

        public IActionResult Detay(int? id)
        {
            var result = _yorumService.Query().FirstOrDefault(x => x.AktifMi && x.Id == id);
            YorumViewModel yvm = new YorumViewModel()
            {
                YorumModel = result
            };
            var cevapResult = _yorumCevapService.Query().Where(x => x.YorumId == id && x.AktifMi).OrderByDescending(x => x.Id).ToList();
            if (cevapResult.Count > 0)
                yvm.YorumCevaplar = cevapResult;
            return View(yvm);
        }


        [Authorize(Roles = "admin")]
        public IActionResult SoftDelete(int? id)
        {
            var result = _yorumService.SoftDelete(id.Value);
            if (result.IsSuccessful)
            {
                TempData["Result"] = "success";
                TempData["Message"] = "İşlem Başarılı";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["Result"] = "danger";
                TempData["Message"] = $"İşlem Başarısız {result.Message}";
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index", "Home");
        }

        private bool? ImajKaydet(YorumModel model, IFormFile yuklenenImaj, bool uzerineYazilsinMi = false)
        {
            bool? sonuc = null;
            string yuklenenDosyaAdi = null, yuklenenDosyaUzantisi = null;
            if (yuklenenImaj != null && yuklenenImaj.Length > 0)
            {
                sonuc = false;
                yuklenenDosyaAdi = yuklenenImaj.FileName;
                yuklenenDosyaUzantisi = Path.GetExtension(yuklenenDosyaAdi);
                string[] imajDosyaUzantilari = AppSettings.ImajDosyaUzantilari.Split(',');
                foreach (string imajDosyaUzantisi in imajDosyaUzantilari)
                {
                    if (yuklenenDosyaUzantisi.ToLower() == imajDosyaUzantisi.ToLower().Trim())
                    {
                        sonuc = true;
                        break;
                    }
                }
                if (sonuc == true)
                {
                    double imajDosyaBoyutu = AppSettings.ImajMaksimumDosyaBoyutu * Math.Pow(1024, 2);
                    if (yuklenenImaj.Length > imajDosyaBoyutu)
                        sonuc = false;
                }
            }


            if (sonuc == true)
            {
                yuklenenDosyaAdi = model.Guid + yuklenenDosyaUzantisi;
                string dosyaYolu = Path.Combine("wwwroot", "dosyalar", "yorumlar", yuklenenDosyaAdi);

                using (FileStream fileStream = new FileStream(dosyaYolu, uzerineYazilsinMi ? FileMode.Create : FileMode.CreateNew))
                {
                    yuklenenImaj.CopyTo(fileStream);
                }
            }
            return sonuc;
        }
    }
}
