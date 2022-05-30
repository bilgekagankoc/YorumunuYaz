using AppCore.Business.Models.Results;
using Business.Models;
using Microsoft.AspNetCore.Mvc;

namespace MvcWebUI.Controllers
{
    [Route("[controller]")]
    public class KategoriController : Controller
    {
        
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult KategoriEkle()
        {
            return View();
        }
        [Route("KategoriEkle/{kategoriModel}")] // ~/SehirlerAjax/SehirlerPost
        [HttpPost]
        public Result KategoriEkle(KategoriModel model)
        {
            if(model == null)
            {
                return new Result(false, "Kategori Bilgisi Boş Olamaz");
            }

            return null;
        }
    }
}
