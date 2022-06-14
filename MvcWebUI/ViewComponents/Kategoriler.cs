using Business.Services;
using Microsoft.AspNetCore.Mvc;

namespace MvcWebUI.ViewComponents
{
    public class Kategoriler : ViewComponent
    {
        private readonly IKategoriService _kategoriService;

        public Kategoriler(IKategoriService kategoriService)
        {
            _kategoriService = kategoriService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var kategoriler = _kategoriService.Query().Where(x => x.AktifMi).ToList();
            return View(kategoriler);
        }
    }
}
