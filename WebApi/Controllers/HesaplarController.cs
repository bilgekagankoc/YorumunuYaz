using Business.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    public class HesaplarController : ControllerBase
    {
        private readonly IHesapService _hesapService;

        public HesaplarController(IHesapService hesapService)
        {
            _hesapService = hesapService;
        }
    }
}
