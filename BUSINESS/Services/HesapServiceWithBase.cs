using AppCore.Business.Models.Results;
using Business.Enums;
using Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public interface IHesapService
    {
        Result<KullaniciModel> Giris(KullaniciGirisModel model);
        Result Kayit(KullaniciKayitModel model);
    }

    public class HesapService : IHesapService
    {
        private readonly IKullaniciService _kullaniciService;

        public HesapService(IKullaniciService kullaniciService)
        {
            _kullaniciService = kullaniciService;
        }

        public Result<KullaniciModel> Giris(KullaniciGirisModel model)
        {
            KullaniciModel kullanici = _kullaniciService.Query().SingleOrDefault(k => k.KullaniciAdi == model.KullaniciAdi && k.Sifre == model.Sifre && k.AktifMi);
            if (kullanici == null)
                return new ErrorResult<KullaniciModel>("Geçersiz kullanıcı adı ve şifre!");
            return new SuccessResult<KullaniciModel>(kullanici);
        }

        public Result Kayit(KullaniciKayitModel model)
        {
            var kullanici = new KullaniciModel()
            {
                AktifMi = true, 
                RolId = model.Rol == 0 ? (int)Rol.Kullanici : (int)model.Rol, // 0 geliyorsa kullanıcı gelmiyorsa gelen değeri setler.
                KullaniciAdi = model.KullaniciAdi,
                Sifre = model.Sifre,
                ePosta = model.ePosta,
            };
            return _kullaniciService.Add(kullanici);
        }
    }
}
