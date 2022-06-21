using Business.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MvcWebUI.ViewComponents
{
    public class YorumYaz : ViewComponent
    {
        private readonly IKategoriService _kategoriService;

        public YorumYaz(IKategoriService kategoriService)
        {
            _kategoriService = kategoriService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var kategoriler = _kategoriService.Query().Where(x => x.AktifMi).ToList();
            ViewBag.Kategoriler = new SelectList(kategoriler,"Id","Ad");
            return View();
        }
    }
}
