using AppCore.Business.Models.Ordering;
using AppCore.Business.Models.Paging;
using AppCore.Business.Models.Results;
using Business.Models;
using Business.Models.Filters;
using Business.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MvcWebUI.Models;
using MvcWebUI.Settings;

namespace MvcWebUI.Controllers
{
    [Authorize(Roles = "admin")]
    public class YorumRaporController : Controller
    {
        private readonly IYorumService _yorumService;
        private readonly IKategoriService _kategoriService;

        public YorumRaporController(IYorumService yorumService, IKategoriService kategoriService)
        {
            _yorumService = yorumService;
            _kategoriService = kategoriService;
        }
        public async Task<IActionResult> Index(int? kategoriId)
        {
            #region Filtreleme
            YorumRaporFilterModel filtre = new YorumRaporFilterModel()
            {
                KategoriId = kategoriId
            };
            #endregion

            #region Sayfalama
            PageModel sayfa = new PageModel()
            {
                RecordsPerPageCount = AppSettings.SayfaKayitSayisi
            };
            #endregion

            #region Sıralama
            OrderModel sira = new OrderModel()
            {
                //DirectionAscending = true, // özelliğin default değeri true
                Expression = "Kullanıcı Adı"
            };
            List<SelectListItem> siraSutunBasliklariSelectListItems = new List<SelectListItem>()
            {
                new SelectListItem()
                {
                    Value = "Kategori",
                    Text = "Kategori"
                },
                new SelectListItem()
                {
                    Value = "Yorum Başlık",
                    Text = "Yorum Başlık"
                },
                new SelectListItem()
                {
                    Value = "Kullanıcı Adı",
                    Text = "Kullanıcı Adı"
                },
                new SelectListItem()
                {
                    Value = "Yorum Icerik",
                    Text = "Yorum Icerik"
                },
                new SelectListItem()
                {
                    Value = "Tarih",
                    Text = "Tarih"
                }
            };
            List<SelectListItem> siraYonSelectListItems = new List<SelectListItem>()
            {
                new SelectListItem()
                {
                    Value = "True",
                    Text = "Artan"
                },
                new SelectListItem()
                {
                    Value = "False",
                    Text = "Azalan"
                }
            };
            #endregion

            Result<List<YorumRaporModel>> result = await _yorumService.RaporGetirAsync(filtre, sayfa, sira);
            ViewBag.Sonuc = result.Message;

            YorumRaporViewModel viewModel = new YorumRaporViewModel()
            {
                YorumlarRaporlar = result.Data,
                YorumFilte = filtre,
                KategorilerMultiSelectList = new MultiSelectList(_kategoriService.Query().ToList(), "Id", "Ad"),
                SayfalarSelectList = new SelectList(sayfa.Pages, "Value", "Text"),
                SiraSutunBasliklariSelectList = new SelectList(siraSutunBasliklariSelectListItems, "Value", "Text"),
                SiraYonSelectList = new SelectList(siraYonSelectListItems, "Value", "Text"),
                Sira = sira
            };

            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(YorumRaporViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                #region Sayfalama
                viewModel.Sayfa.RecordsPerPageCount = AppSettings.SayfaKayitSayisi;
                #endregion

                #region Sıralama
                List<SelectListItem> siraSutunBasliklariSelectListItems = new List<SelectListItem>()
                {
                new SelectListItem()
                {
                    Value = "Kategori",
                    Text = "Kategori"
                },
                new SelectListItem()
                {
                    Value = "Yorum Başlık",
                    Text = "Yorum Başlık"
                },
                new SelectListItem()
                {
                    Value = "Kullanıcı Adı",
                    Text = "Kullanıcı Adı"
                },
                new SelectListItem()
                {
                    Value = "Yorum Icerik",
                    Text = "Yorum Icerik"
                },
                new SelectListItem()
                {
                    Value = "Tarih",
                    Text = "Tarih"
                }
                };
                List<SelectListItem> siraYonSelectListItems = new List<SelectListItem>()
                {
                    new SelectListItem()
                    {
                        Value = "True",
                        Text = "Artan"
                    },
                    new SelectListItem()
                    {
                        Value = "False",
                        Text = "Azalan"
                    }
                };
                #endregion

                Result<List<YorumRaporModel>> result = await _yorumService.RaporGetirAsync(viewModel.YorumFilte, viewModel.Sayfa, viewModel.Sira);
                ViewBag.Sonuc = result.Message;
                viewModel.YorumlarRaporlar = result.Data;

                // partial view'da kullanılan sayfa ve sıra tekrar doldurulmalıdır ki partial view bunlar üzerinden güncellenebilsin
                // Sayfalama
                viewModel.SayfalarSelectList = new SelectList(viewModel.Sayfa.Pages, "Value", "Text", viewModel.Sayfa.PageNumber);

                // Sıralama
                viewModel.SiraSutunBasliklariSelectList = new SelectList(siraSutunBasliklariSelectListItems, "Value", "Text", viewModel.Sira.Expression);
                viewModel.SiraYonSelectList = new SelectList(siraYonSelectListItems, "Value", "Text", viewModel.Sira.DirectionAscending);
                viewModel.KategorilerMultiSelectList = new MultiSelectList(_kategoriService.Query().ToList(), "Id", "Ad");
            }
            return View(viewModel);
        }
    }
}
