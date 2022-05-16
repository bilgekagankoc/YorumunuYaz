using DataAccess.Contexts;
using DataAccess.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace MvcWebUI.Controllers
{
    public class DatabaseController : Controller
    {
        public IActionResult Seed()
        {
            using (var dbcontext = new YorumunuYazContext())
            {
                var kullanici = dbcontext.Kullanicilar.ToList();
                dbcontext.Kullanicilar.RemoveRange(kullanici);
                var rol = dbcontext.Roller.ToList();
                dbcontext.Roller.RemoveRange(rol);

                dbcontext.Database.ExecuteSqlRaw("dbcc CHECKIDENT ('Kullanicilar', RESEED, 0)");
                dbcontext.Database.ExecuteSqlRaw("dbcc CHECKIDENT ('Roller', RESEED, 0)");

                List<Kullanici> kullanicilar = new List<Kullanici>()
                {
                    new Kullanici()
                    {
                        AktifMi=true,
                        KullaniciAdi="escepto",
                        Guid=Guid.NewGuid().ToString(),
                        Sifre="122333",
                        ePosta ="asd@asd.com",
                        Rol=new Rol()
                            {
                            Adi="admin",
                            Guid=Guid.NewGuid().ToString()
                        },
                    },
                    new Kullanici()
                    {
                        AktifMi=true,
                        KullaniciAdi="konix",
                        ePosta ="asd@asdasd.com",
                        Guid=Guid.NewGuid().ToString(),
                        Sifre="122333",
                        Rol=new Rol()
                            {
                            Adi="kullanici",
                            Guid=Guid.NewGuid().ToString()
                        }
                    }
                };
                foreach (var item in kullanicilar)
                {
                    dbcontext.Kullanicilar.Add(item);
                }
                dbcontext.SaveChanges();
            }
            
            return Content("<label style=\"color:red;\"><b>İlk veriler oluşturuldu.</b></label>", "text/html", Encoding.UTF8);
        }
    }
}
