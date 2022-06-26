using AppCore.Business.Models.Ordering;
using AppCore.Business.Models.Paging;
using AppCore.Business.Models.Results;
using AppCore.Business.Services.Bases;
using AppCore.DataAccess.EntityFramework;
using AppCore.DataAccess.EntityFramework.Bases;
using Business.Models;
using Business.Models.Filters;
using DataAccess.Contexts;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public interface IYorumService : IService<YorumModel, Yorum, YorumunuYazContext>
    {
        Task<Result<List<YorumRaporModel>>> RaporGetirAsync(YorumRaporFilterModel filtre, PageModel sayfa, OrderModel sira);
    }

    public class YorumService : IYorumService
    {
        public RepoBase<Yorum, YorumunuYazContext> Repo { get; set; }
        public RepoBase<Kullanici, YorumunuYazContext> _kullaniciService { get; set; }
        public RepoBase<YorumCevap, YorumunuYazContext> _yorumCevapService { get; set; }
        public RepoBase<Kategori, YorumunuYazContext> _kategoriService { get; set; }
        public YorumService()
        {
            YorumunuYazContext dbContext = new YorumunuYazContext();
            Repo = new Repo<Yorum, YorumunuYazContext>(dbContext);
            _kullaniciService = new Repo<Kullanici, YorumunuYazContext>(dbContext);
            _yorumCevapService = new Repo<YorumCevap, YorumunuYazContext>(dbContext);
            _kategoriService = new Repo<Kategori, YorumunuYazContext>(dbContext);
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
                CevapSayisiDisplay = x.YorumCevaplar.Select(x => x.YorumId == x.Id).Count(),
                ImajYoluDisplay = string.IsNullOrWhiteSpace(x.ImajDosyaUzantisi) ? null : "/dosyalar/yorumlar/" + x.Guid + x.ImajDosyaUzantisi,
                OlusturmaTarihiDisplay = x.OlusturmaTarih.Value.ToString("MM/dd/yyyy")
            });
        }

        public Result Add(YorumModel model)
        {
            if (Repo.Query().Any(r => r.Baslik.ToLower() == model.Baslik.ToLower().Trim() && r.KategoriId == model.KategoriId))
                return new ErrorResult("Bu Kategoride Aynı İçeriğe Sahip Başlık Mevcut. Mevcut İçeriğe Cevap Yazabilirsiniz.");
            Yorum entity = new Yorum()
            {
                AktifMi = true,
                Baslik = model.Baslik.ToUpper().Trim(),
                Icerik = model.Icerik,
                ImajDosyaUzantisi = model.ImajDosyaUzantisi,
                KategoriId = model.KategoriId,
                KullaniciId = model.OlusturanKullaniciId.Value,
                OlusturanKullaniciId = model.OlusturanKullaniciId,
                OlusturmaTarih = DateTime.Now,
                Guid = model.Guid
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
            var entity = Repo.Query(x => x.Id == id).FirstOrDefault();
            entity.AktifMi = false;
            Repo.Update(entity);
            return new SuccessResult();
        }

        public void Dispose()
        {
            Repo.Dispose();
        }

        public async Task<Result<List<YorumRaporModel>>> RaporGetirAsync(YorumRaporFilterModel filtre, PageModel sayfa, OrderModel sira)
        {
            List<YorumRaporModel> list;

            var yorumQuery = Repo.Query();
            var yorumCevapQuery = _yorumCevapService.Query();
            var kullaniciQuery = _kullaniciService.Query();
            var kategoriQuery = _kategoriService.Query();

            var query = from yorum in yorumQuery
                        join kategori in kategoriQuery
                        on yorum.KategoriId equals kategori.Id into kategoriler
                        from subKategoriler in kategoriler.DefaultIfEmpty()
                        join yorumCevap in yorumCevapQuery
                        on yorum.Id equals yorumCevap.YorumId into yorumCevaplar
                        from subYorumCevaplar in yorumCevaplar.DefaultIfEmpty()
                        join kullanici in kullaniciQuery
                        on yorum.KullaniciId equals kullanici.Id into kullanicilar
                        from subKullanicilar in kullanicilar.DefaultIfEmpty()
                        select new YorumRaporModel()
                        {
                            KategoriAdi = subKategoriler.Ad,
                            KullaniciAdi = subKullanicilar.KullaniciAdi,
                            KullaniciId = subKullanicilar.Id,
                            TarihDisplay = yorum.OlusturmaTarih.Value.ToString("dd/MM/yyyy"),
                            YorumBaslik = yorum.Baslik,
                            YorumIcerik = yorum.Icerik,
                            YorumId = yorum.Id,
                            Tarih = yorum.OlusturmaTarih.Value,
                            KategoriId =subKategoriler.Id
                        };
            switch (sira.Expression)
            {
                case "Kategori":
                    query = sira.DirectionAscending
                        ? query.OrderBy(q => q.KategoriAdi)
                        : query.OrderByDescending(q => q.KategoriAdi);
                    break;
                case "Yorum Başlık":
                    query = sira.DirectionAscending
                        ? query.OrderBy(q => q.YorumBaslik)
                        : query.OrderByDescending(q => q.YorumBaslik);
                    break;
                case "Kullanıcı Adı":
                    query = sira.DirectionAscending
                        ? query.OrderBy(q => q.KullaniciAdi)
                        : query.OrderByDescending(q => q.KullaniciAdi);
                    break;
                case "Yorum Icerik":
                    query = sira.DirectionAscending
                        ? query.OrderBy(q => q.YorumIcerik)
                        : query.OrderByDescending(q => q.YorumIcerik);
                    break;
                case "Tarih":
                    query = sira.DirectionAscending
                        ? query.OrderBy(q => q.Tarih)
                        : query.OrderByDescending(q => q.Tarih);
                    break;
                default:
                    query = sira.DirectionAscending
                        ? query.OrderBy(q => q.YorumId)
                        : query.OrderByDescending(q => q.YorumId);
                    break;
            }
            if (filtre.KategoriId.HasValue)
                query = query.Where(q => q.KategoriId == filtre.KategoriId.Value);

            if (!string.IsNullOrWhiteSpace(filtre.KullaniciAdi))
                query = query.Where(q => q.KullaniciAdi.ToLower() == filtre.KullaniciAdi.ToLower());

            if (!string.IsNullOrWhiteSpace(filtre.YorumBaslik))
                query = query.Where(q => q.YorumBaslik.ToLower().Contains(filtre.YorumBaslik.ToLower()));

            if (!string.IsNullOrWhiteSpace(filtre.YorumIcerik))
                query = query.Where(q => q.YorumIcerik.ToLower().Contains(filtre.YorumIcerik.ToLower()));

            if (filtre.Tarih.HasValue)
                query = query.Where(q => q.Tarih.Value.ToShortTimeString() == filtre.Tarih.Value.ToShortTimeString());
            sayfa.RecordsCount = query.Count();
            int skip = (sayfa.PageNumber - 1) * sayfa.RecordsPerPageCount; 
            int take = sayfa.RecordsPerPageCount; 
            query = query.Skip(skip).Take(take); 
            list = await query.ToListAsync();
            if (list.Count == 0)
                return new ErrorResult<List<YorumRaporModel>>("Kayıt bulunamadı!");
            return new SuccessResult<List<YorumRaporModel>>(sayfa.RecordsCount + " kayıt bulundu.", list);
        }
    }
}
