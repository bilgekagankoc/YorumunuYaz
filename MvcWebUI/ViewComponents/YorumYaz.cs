using Business.Services;
using Microsoft.AspNetCore.Mvc;

namespace MvcWebUI.ViewComponents
{
    public class YorumYaz : ViewComponent
    {
        private readonly IYorumService _yorumService;
        private readonly IYorumCevapService _yorumCevapService;

        public YorumYaz(IYorumService yorumService, IYorumCevapService yorumCevapService)
        {
            _yorumService = yorumService;
            _yorumCevapService = yorumCevapService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
