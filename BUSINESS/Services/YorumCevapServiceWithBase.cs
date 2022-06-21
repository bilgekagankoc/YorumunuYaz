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
    public interface IYorumCevapService : IService<YorumCevapModel,YorumCevap,YorumunuYazContext>
    {

    }
    public class YorumCevapService : IYorumCevapService
    {
        public RepoBase<YorumCevap, YorumunuYazContext> Repo { get; set; } 
        public RepoBase<Kullanici, YorumunuYazContext> _kullaniciService { get; set; }

        public YorumCevapService()
        {
            YorumunuYazContext dbContext = new YorumunuYazContext();
            Repo = new Repo<YorumCevap, YorumunuYazContext>(dbContext);
            _kullaniciService = new Repo<Kullanici, YorumunuYazContext>(dbContext);
        }


        public IQueryable<YorumCevapModel> Query()
        {
            return Repo.Query().OrderBy(x => x.OlusturmaTarih).Select(x => new YorumCevapModel()
            {
                Cevap = x.Cevap,
                AktifMi = x.AktifMi,
                OlusturmaTarih = x.OlusturmaTarih,
                GuncellemeTarih = x.GuncellemeTarih,
                GuncelleyenKullaniciId = x.GuncelleyenKullaniciId,
                OlusturanKullaniciId = x.OlusturanKullaniciId,
                Yorum = x.Yorum,
                YorumId = x.YorumId,
                KullaniciId = x.KullaniciId,
                Kullanici = x.Kullanici,
                OlusturanKullaniciAdiDisplay = _kullaniciService.Query().FirstOrDefault(k => k.Id == x.OlusturanKullaniciId).KullaniciAdi,
                GüncelleyenKullaniciAdiDisplay = _kullaniciService.Query().FirstOrDefault(k => k.Id == x.GuncelleyenKullaniciId).KullaniciAdi,
                Id = x.Id,
                OlusturmaTarihDisplay = x.OlusturmaTarih.Value.ToString("MM/dd/yyyy")
            }); ;
        }

        public Result Add(YorumCevapModel model)
        {
            YorumCevap entity = new YorumCevap()
            {
                Cevap = model.Cevap,
                AktifMi = true,
                OlusturanKullaniciId = model.OlusturanKullaniciId,
                KullaniciId = model.KullaniciId,
                OlusturmaTarih = DateTime.Now,
                Guid = Guid.NewGuid().ToString(),
                YorumId = model.YorumId
            };
            Repo.Add(entity);
            return new SuccessResult();
        }

        public Result Update(YorumCevapModel model)
        {
            YorumCevap entity = Repo.Query().FirstOrDefault(x => x.Id == model.Id);
            entity.Cevap = model.Cevap;
            entity.GuncelleyenKullaniciId = model.GuncelleyenKullaniciId;
            entity.GuncellemeTarih = DateTime.Now;
            Repo.Update(entity);
            return new SuccessResult();
        }

        public Result Delete(int id)
        {
            YorumCevap entity = Repo.Query().FirstOrDefault(x => x.Id == id);
            Repo.Delete(entity);
            return new SuccessResult();
        }

        public Result SoftDelete(int id)
        {
            var entity = Repo.Query(x => x.Id == id).FirstOrDefault();
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
