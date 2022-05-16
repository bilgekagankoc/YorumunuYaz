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
    public interface IRolService : IService<RolModel, Rol, YorumunuYazContext>
    {
        Result<List<RolModel>> RolleriGetir();
        Result<RolModel> RolGetir(int id);
    }

    public class RolService : IRolService
    {
        public RepoBase<Rol, YorumunuYazContext> Repo { get; set; } = new Repo<Rol, YorumunuYazContext>();

        public IQueryable<RolModel> Query()
        {
            return Repo.Query("Kullanicilar").OrderBy(x => x.Adi).Select(x => new RolModel()
            {
                Id = x.Id,
                Adi = x.Adi,
                KullanicilarDisplay = x.Kullanicilar.Select(x => x.KullaniciAdi).ToList()
            });
        }

        public Result Add(RolModel model)
        {
            if (Repo.Query().Any(r => r.Adi.ToUpper() == model.Adi.ToUpper().Trim()))
                return new ErrorResult("Aynı Rol Adına Sahip Rol Bulunmaktadır!");
            Rol entity = new Rol()
            {
                Adi = model.Adi.Trim()
            };
            Repo.Add(entity);
            return new SuccessResult();
        }

        public Result Update(RolModel model)
        {
            if (Repo.Query().Any(r => r.Adi.ToUpper() == model.Adi.ToUpper().Trim() && r.Id != model.Id))
                return new ErrorResult("Aynı Rol Adına Sahip Rol Bulunmaktadır!");
            Rol entity = Repo.Query(r => r.Id == model.Id).SingleOrDefault();
            entity.Adi = model.Adi.Trim();
            Repo.Update(entity);
            return new SuccessResult();
        }

        public Result Delete(int id)
        {
            Rol entity = Repo.Query(r => r.Id == id, "Kullanicilar").SingleOrDefault();
            if (entity.Kullanicilar != null && entity.Kullanicilar.Count > 0)
                return new ErrorResult("Silinmek istenen role ait kullanıcılar bulunmaktadır!");
            Repo.Delete(entity);
            return new SuccessResult();
        }

        public void Dispose()
        {
            Repo.Dispose();
        }

        public Result<RolModel> RolGetir(int id)
        {
            var rol = Query().SingleOrDefault(r => r.Id == id);
            if (rol == null)
                return new ErrorResult<RolModel>("Rol bulunamadı!");
            return new SuccessResult<RolModel>(rol);
        }

        public Result<List<RolModel>> RolleriGetir()
        {
            var roller = Query().ToList();
            if (roller.Count == 0)
                return new ErrorResult<List<RolModel>>("Rol bulunamadı!");
            return new SuccessResult<List<RolModel>>(roller.Count + " rol bulundu.", roller);
        }


    }
}
