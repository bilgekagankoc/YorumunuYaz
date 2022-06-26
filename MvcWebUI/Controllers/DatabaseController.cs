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

                var yorumCevap = dbcontext.YorumCevaplar.ToList();
                dbcontext.YorumCevaplar.RemoveRange(yorumCevap);

                var yorum = dbcontext.Yorumlar.ToList();
                dbcontext.Yorumlar.RemoveRange(yorum);


                var kategori = dbcontext.Kategoriler.ToList();
                dbcontext.Kategoriler.RemoveRange(kategori);

                var kullanici = dbcontext.Kullanicilar.ToList();
                dbcontext.Kullanicilar.RemoveRange(kullanici);
                var rol = dbcontext.Roller.ToList();
                dbcontext.Roller.RemoveRange(rol);


                


                dbcontext.Database.ExecuteSqlRaw("dbcc CHECKIDENT ('Kullanicilar', RESEED, 0)");
                dbcontext.Database.ExecuteSqlRaw("dbcc CHECKIDENT ('Roller', RESEED, 0)");

                dbcontext.Roller.Add(new Rol()
                {
                    Adi = "admin",
                    Guid = Guid.NewGuid().ToString(),
                    AktifMi = true,
                    OlusturmaTarih = DateTime.Now
                });
                dbcontext.Roller.Add(new Rol()
                {
                    Adi = "kulanici",
                    Guid = Guid.NewGuid().ToString(),
                    AktifMi = true,
                    OlusturmaTarih = DateTime.Now
                });
                dbcontext.SaveChanges();
                dbcontext.Kullanicilar.Add(new Kullanici()
                {
                    AktifMi = true,
                    KullaniciAdi = "escepto",
                    Guid = Guid.NewGuid().ToString(),
                    Sifre = "122333",
                    ePosta = "asd@asd.com",
                    OlusturmaTarih = DateTime.Now,
                    RolId = 1,
                });
                dbcontext.Kullanicilar.Add(new Kullanici()
                {
                    AktifMi = true,
                    KullaniciAdi = "konix",
                    ePosta = "asd@asdasd.com",
                    Guid = Guid.NewGuid().ToString(),
                    Sifre = "122333",
                    OlusturmaTarih = DateTime.Now,
                    RolId = 2,
                });
                dbcontext.SaveChanges();
                dbcontext.Kategoriler.Add(new Kategori()
                {
                    Aciklama = "Otomotiv hakkında herşey",
                    Ad = "Otomotiv",
                    Guid = Guid.NewGuid().ToString(),
                    AktifMi = true,
                    OlusturanKullaniciId = 1,
                    OlusturmaTarih = DateTime.Now,
                });
                dbcontext.Kategoriler.Add(new Kategori()
                {
                    Aciklama = "Elektronik hakkında herşey",
                    Ad = "Elektronik",
                    Guid = Guid.NewGuid().ToString(),
                    AktifMi = true,
                    OlusturanKullaniciId = 2,
                    OlusturmaTarih = DateTime.Now,
                });

                dbcontext.SaveChanges();
            }

            return Content("<label style=\"color:red;\"><b>İlk veriler oluşturuldu.</b></label>", "text/html", Encoding.UTF8);
        }
    }
}
