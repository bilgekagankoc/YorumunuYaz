using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Models
{
    public class YorumRaporModel
    {
        [DisplayName("Yorum Başlık")]
        public string YorumBaslik { get; set; }
        [DisplayName("Yorum Cevap")]
        public string YorumCevap { get; set; }
        [DisplayName("Yorum İçerik")]
        public string YorumIcerik { get; set; }
        [DisplayName("Kategori")]
        public string KategoriAdi { get; set; }
        [DisplayName("Kullanıcı Adı")]
        public string KullaniciAdi { get; set; }
        [DisplayName("Oluşturma Tarihi")]
        public string TarihDisplay { get; set; }
        
        [DisplayName("Tarih")]
        public DateTime? Tarih { get; set; }
        [DisplayName("Cevap Sayısı")]
        public int CevapSayisi { get; set; }
        public int YorumId { get; set; }
        
        public int YorumCevapId { get; set; }
        public int KullaniciId { get; set; }
        public int? KategoriId { get; set; }
    }
}
