using Microsoft.AspNetCore.Mvc;

namespace MvcWebUI.Controllers
{
    public class YorumController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
