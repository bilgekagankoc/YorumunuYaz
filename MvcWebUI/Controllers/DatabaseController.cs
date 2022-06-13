﻿using DataAccess.Contexts;
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
                        OlusturanKullaniciId=1,
                        OlusturmaTarih=DateTime.Now,
                        RolId=1,
                        Rol=new Rol()
                            {
                            Adi="admin",
                            Guid=Guid.NewGuid().ToString(),
                            AktifMi = true,
                            OlusturanKullaniciId=1,
                            OlusturmaTarih = DateTime.Now
                        },
                    },
                    new Kullanici()
                    {
                        AktifMi=true,
                        KullaniciAdi="konix",
                        ePosta ="asd@asdasd.com",
                        Guid=Guid.NewGuid().ToString(),
                        Sifre="122333",
                        OlusturanKullaniciId=1,
                        OlusturmaTarih=DateTime.Now,
                        RolId=2,
                        Rol=new Rol()
                            {
                            Adi="kullanici",
                            AktifMi = true,
                            Guid=Guid.NewGuid().ToString(),
                            OlusturanKullaniciId=1,
                            OlusturmaTarih = DateTime.Now
                        }
                    }
                };
                Kategori k1 = new Kategori()
                {
                    Aciklama = "Otomotiv hakkında herşey",
                    Ad = "Otomotiv",
                    Guid = Guid.NewGuid().ToString(),
                    AktifMi = true,
                    OlusturanKullaniciId = 1,
                    OlusturmaTarih = DateTime.Now,
                };
                Kategori k2 = new Kategori()
                {
                    Aciklama = "Elektronik hakkında herşey",
                    Ad = "Elektronik",
                    Guid = Guid.NewGuid().ToString(),
                    AktifMi = true,
                    OlusturanKullaniciId = 2,
                    OlusturmaTarih = DateTime.Now,
                };
                foreach (var item in kullanicilar)
                {
                    dbcontext.Kullanicilar.Add(item);
                }
                dbcontext.Kategoriler.Add(k1);
                dbcontext.Kategoriler.Add(k2);
                dbcontext.SaveChanges();
            }

            return Content("<label style=\"color:red;\"><b>İlk veriler oluşturuldu.</b></label>", "text/html", Encoding.UTF8);
        }
    }
}
