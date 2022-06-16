using AppCore.Business.Models.Results;
using AppCore.Business.Services.Bases;
using AppCore.DataAccess.EntityFramework;
using AppCore.DataAccess.EntityFramework.Bases;
using Business.Models;
using DataAccess.Contexts;
using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public interface IYorumService : IService<YorumModel, Yorum, YorumunuYazContext>
    {

    }

    public class YorumService : IYorumService
    {
        public RepoBase<Yorum, YorumunuYazContext> Repo { get; set; }

        public RepoBase<Kullanici, YorumunuYazContext> _kullaniciService { get; set; }

        public YorumService()
        {
            YorumunuYazContext dbContext = new YorumunuYazContext();
            Repo = new Repo<Yorum, YorumunuYazContext>(dbContext);
            _kullaniciService = new Repo<Kullanici, YorumunuYazContext>(dbContext);
        }

        public IQueryable<YorumModel> Query()
        {
            return Repo.Query("YorumCevaplar").OrderBy(x => x.OlusturmaTarih).Select(x => new YorumModel()
            {
                AktifMi = x.AktifMi,
                Baslik = x.Baslik,
                OlusturmaTarih = x.OlusturmaTarih,
                GuncellemeTarih = x.GuncellemeTarih,
                Icerik = x.Icerik,
                Id = x.Id,
                ImajDosyaUzantisi = x.ImajDosyaUzantisi,
                Kategori = x.Kategori,
                KategoriAdDisplay = x.Kategori.Ad,
                KategoriId = x.KategoriId,
                Kullanici = x.Kullanici,
                KullaniciId = x.KullaniciId,
                GuncelleyenKullaniciId = x.GuncelleyenKullaniciId,
                OlusturanKullaniciId = x.OlusturanKullaniciId,
                OlusturanKullaniciAdiDisplay = _kullaniciService.Query().FirstOrDefault(k => k.Id == x.OlusturanKullaniciId).KullaniciAdi,
                GüncelleyenKullaniciAdiDisplay = _kullaniciService.Query().FirstOrDefault(k => k.Id == x.GuncelleyenKullaniciId).KullaniciAdi,
                CevapSayisiDisplay = x.YorumCevaplar.Select(x => x.YorumId == x.Id).Count()
            });
        }

        public Result Add(YorumModel model)
        {
            if (Repo.Query().Any(r => r.Baslik.ToLower() == model.Baslik.ToLower().Trim()))
                return new ErrorResult("Aynı İçeriğe Sahip İçerik Mevcut. Mevcut İçeriğe Cevap Yazabilirsiniz.");
            Yorum entity = new Yorum()
            {
                AktifMi = true,
                Baslik = model.Baslik.ToUpper().Trim(),
                Guid = Guid.NewGuid().ToString(),
                Icerik = model.Icerik,
                ImajDosyaUzantisi = model.ImajDosyaUzantisi,
                KategoriId = model.KategoriId,
                KullaniciId = model.OlusturanKullaniciId.Value,
                OlusturanKullaniciId = model.OlusturanKullaniciId,
                OlusturmaTarih = DateTime.Now,
            };
            Repo.Add(entity);
            return new SuccessResult();
        }

        public Result Update(YorumModel model)
        {
            if (Repo.Query().Any(r => r.Baslik.ToLower() == model.Baslik.ToLower().Trim() && r.Id != model.Id))
                return new ErrorResult("Değiştirmek İstediğiniz Başlık Mevcut. Farklı Bir Başlık Giriniz.");
            Yorum entity = Repo.Query().FirstOrDefault(x => x.Id == model.Id);
            entity.Baslik = model.Baslik.ToUpper().Trim();
            entity.Icerik = model.Icerik;
            entity.GuncellemeTarih = DateTime.Now;
            entity.GuncelleyenKullaniciId = model.GuncelleyenKullaniciId;
            Repo.Update(entity);
            Repo.Update(entity);
            return new SuccessResult();
        }

        public Result Delete(int id)
        {
            Yorum entity = Repo.Query(r => r.Id == id, "YorumCevaplar").SingleOrDefault();
            if (entity.YorumCevaplar != null && entity.YorumCevaplar.Count > 0)
                return new ErrorResult("Silinmek istenen yoruma ait cevaplar bulunmaktadır!");
            Repo.Delete(entity);
            return new SuccessResult();
        }

        public Result SoftDelete(int id)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            Repo.Dispose();
        }
    }
}
