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
    public interface IKullaniciService :IService<KullaniciModel, Kullanici, YorumunuYazContext>
    {
        Result<List<KullaniciModel>> KullanicilariGetir();
        Result<KullaniciModel> KullaniciGetir(int id);
    }

    public class KullaniciService : IKullaniciService
    {
        public RepoBase<Kullanici, YorumunuYazContext> Repo { get; set; }
        public RepoBase<Rol,YorumunuYazContext> _rolRepo { get; set; }

        public KullaniciService()
        {
            YorumunuYazContext dbContext = new YorumunuYazContext();
            Repo = new Repo<Kullanici, YorumunuYazContext>(dbContext);
            _rolRepo = new Repo<Rol, YorumunuYazContext>(dbContext);
        }
        public IQueryable<KullaniciModel> Query()
        {
            var rolQuery = _rolRepo.Query();
            var kullaniciQuery = Repo.Query();

            var query = from kullanici in kullaniciQuery
                        join rol in rolQuery on kullanici.RolId equals rol.Id
                        orderby rol.Adi, kullanici.KullaniciAdi
                        select new KullaniciModel()
                        {
                            Id = kullanici.Id,
                            KullaniciAdi = kullanici.KullaniciAdi,
                            RolId = kullanici.RolId,
                            RolAdiDisplay = rol.Adi,
                            AktifMi = kullanici.AktifMi,
                            AktifDisplay = kullanici.AktifMi ? "Evet" : "Hayır",
                            Sifre = kullanici.Sifre
                        };
            return query;
        }

        public Result Add(KullaniciModel model)
        {
            if (Repo.Query().Any(k => k.KullaniciAdi.ToUpper() == model.KullaniciAdi.ToUpper().Trim()))
                return new ErrorResult("Kullanıcı adı başkası tarafından kullanılmaktadır!");
            if (Repo.Query().Any(k => k.ePosta.ToUpper() == model.ePosta.ToUpper().Trim()))
                return new ErrorResult("E-Posta başkası tarafından kullanılmaktadır!");
            var entity = new Kullanici()
            {
                ePosta = model.ePosta,
                KullaniciAdi = model.KullaniciAdi,
                RolId = model.RolId.Value,
                Sifre = model.Sifre,
            };
            Repo.Add(entity);
            return new SuccessResult();
        }

        public Result Update(KullaniciModel model)
        {
            if (Repo.Query().Any(x => x.KullaniciAdi.ToUpper() == model.KullaniciAdi.ToUpper().Trim() && x.Id != model.Id))
                return new ErrorResult("Kullanıcı adı başkası tarafından kullanılmaktadır!");
            if (Repo.Query("KullaniciDetayi").Any(x => x.ePosta.ToUpper() == model.ePosta.ToUpper().Trim() && x.Id != model.Id))
                return new ErrorResult("E-Posta başkası tarafından kullanılmaktadır!");
            var entity = Repo.Query(x=>x.Id == model.Id).SingleOrDefault();
            if(entity != null)
            {
                entity.Sifre = model.Sifre;
                entity.ePosta = model.ePosta;
                entity.KullaniciAdi = model.KullaniciAdi;
                entity.RolId = model.RolId.Value;
                Repo.Update(entity);
                return new SuccessResult();
            }
            return new ErrorResult("Kullanıcı bulunamadı!");
        }

        public Result Delete(int id)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            Repo.Dispose();
        }

        public Result<List<KullaniciModel>> KullanicilariGetir()
        {
            var kullanicilar = Query().ToList();
            if (kullanicilar.Count == 0)
                return new ErrorResult<List<KullaniciModel>>("Kullanıcı bulunamadı!");
            return new SuccessResult<List<KullaniciModel>>(kullanicilar.Count + " kullanıcı bulundu.", kullanicilar);
        }

        public Result<KullaniciModel> KullaniciGetir(int id)
        {
            var kullanici = Query().SingleOrDefault(k => k.Id == id);
            if (kullanici == null)
                return new ErrorResult<KullaniciModel>("Kullanıcı bulunamadı!");
            return new SuccessResult<KullaniciModel>(kullanici);
        }

    }
}
