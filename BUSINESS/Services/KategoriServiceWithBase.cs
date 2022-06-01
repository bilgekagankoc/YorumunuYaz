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
    public interface IKategoriService : IService<KategoriModel ,Kategori,YorumunuYazContext>
    {

    }

    public class KategoriService : IKategoriService
    {
        public RepoBase<Kategori, YorumunuYazContext> Repo { get; set; } = new Repo<Kategori, YorumunuYazContext>();

        public IQueryable<KategoriModel> Query()
        {
            return Repo.Query("Yorumlar").OrderBy(x => x.Ad).Select(x => new KategoriModel()
            {
                Aciklama = x.Aciklama,
                Ad = x.Ad,
                AktifMi = x.AktifMi,
                GuncellemeTarih = x.GuncellemeTarih,
                GuncelleyenKullaniciId = x.GuncelleyenKullaniciId,
                OlusturanKullaniciId = x.OlusturanKullaniciId,
                Yorumlar = x.Yorumlar,
                OlusturmaTarih = x.OlusturmaTarih,
                YorumSayısı = x.Yorumlar.Count()
            });
        }

        public Result Add(KategoriModel model)
        {
            if (Repo.Query().Any(r => r.Ad.ToLower() == model.Ad.ToLower().Trim()))
                return new ErrorResult("Aynı Kategori Adına Sahip Rol Bulunmaktadır!");
            Kategori kategori = new Kategori()
            {
                Aciklama = model.Aciklama,
                Ad = model.Ad,
                Guid = Guid.NewGuid().ToString(),
                AktifMi = true,
                OlusturmaTarih = DateTime.Now,
                OlusturanKullaniciId = model.OlusturanKullaniciId
            };
            Repo.Add(kategori);
            return new SuccessResult();
        }

        public Result Update(KategoriModel model)
        {
            if (Repo.Query().Any(r => r.Ad.ToLower() == model.Ad.ToLower().Trim() && r.Id != model.Id))
                return new ErrorResult("Aynı Kategori Adına Sahip Kategori Bulunmaktadır!");
            Kategori entity = Repo.Query(x => x.Id == model.Id).FirstOrDefault();
            return null;
        }

        public Result Delete(int id)
        {
            Kategori entity = Repo.Query(x => x.Id == id).FirstOrDefault();
            if (entity.Yorumlar != null && entity.Yorumlar.Count > 0)
                return new ErrorResult("Silinmek istenen kategoride yorumlar vardır");
            Repo.Delete(entity);
            return new SuccessResult();
        }
        public Result SoftDelete(int id)
        {
            Kategori entity = Repo.Query(x => x.Id == id).FirstOrDefault();
            entity.AktifMi = false;
            Repo.Update(entity);
            return new SuccessResult();
        }

        public void Dispose()
        {
            Repo.Dispose();
        }


    }
}
