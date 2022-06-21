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
    public class YorumCevapModel : RecordBase
    {
        #region Entityden Kopyalanan Özellikler
        [Required(ErrorMessage = "{0} gereklidir!")]
        [MinLength(1, ErrorMessage = "{0} en az {1} karakter olmalıdır!")]
        [MaxLength(500, ErrorMessage = "{0} en çok {1} karakter olmalıdır!")]
        [DisplayName("Cevap")]
        public string Cevap { get; set; }
        public int KullaniciId { get; set; }
        public Kullanici Kullanici { get; set; }
        public int YorumId { get; set; }
        public Yorum Yorum { get; set; }
        #endregion

        #region Sayfanın ekstra ihtiyaçları
        [DisplayName("Oluşturan Kullanıcı")]
        public string OlusturanKullaniciAdiDisplay { get; set; }
        [DisplayName("Güncelleyen Kullanıcı")]
        public string GüncelleyenKullaniciAdiDisplay { get; set; }
        public string OlusturmaTarihDisplay { get; set; }

        #endregion
    }
}
