using AppCore.Records.Bases;
using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Models
{
    public class YorumModel : RecordBase
    {
        #region Entityden kopyalanan özellikler
        [Required(ErrorMessage = "{0} gereklidir!")]
        [MinLength(1, ErrorMessage = "{0} en az {1} karakter olmalıdır!")]
        [MaxLength(22, ErrorMessage = "{0} en çok {1} karakter olmalıdır!")]
        [DisplayName("Başlık")]
        public string Baslik { get; set; }
        [Required(ErrorMessage = "{0} gereklidir!")]
        [MinLength(1, ErrorMessage = "{0} en az {1} karakter olmalıdır!")]
        [MaxLength(500, ErrorMessage = "{0} en çok {1} karakter olmalıdır!")]
        [DisplayName("İçerik")]
        public string Icerik { get; set; }
        [DisplayName("Eklenen Medya")]
        [StringLength(5)]
        public string ImajDosyaUzantisi { get; set; }
        public int KategoriId { get; set; }
        public Kategori Kategori { get; set; }
        public int KullaniciId { get; set; }
        public Kullanici Kullanici { get; set; }
        #endregion
        #region Sayfanın Ekstra Özellikleri
        [DisplayName("Kategori Adı")]
        public string KategoriAdDisplay { get; set; }
        [DisplayName("Oluşturan Kullanıcı")]
        public string OlusturanKullaniciAdiDisplay { get; set; }

        #endregion
    }
}
