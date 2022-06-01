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
    public class RolModel : RecordBase
    {
        #region Entity'den kopyalanan özellikler
        [Required(ErrorMessage = "{0} gereklidir!")]
        [StringLength(20, ErrorMessage = "{0} en fazla {1} karakter olmalıdır!")]
        [DisplayName("Adı")]
        public string Adi { get; set; }
        #endregion

        #region Sayfanın ekstra ihtiyacı olan özellikler
        [DisplayName("Kullanıcılar")]
        public List<string> Kullanicilar { get; set; }
        [DisplayName("Oluşturan Kullanıcı Adı")]
        public string OlusturanKullaniciAdi { get; set; }
        [DisplayName("Güncelleyen Kullanıcı Adı")]
        public string GuncelleyenKullaniciAdi { get; set; }
        [DisplayName("Aktif Mi")]
        public string AktifMiDisplay { get; set; }
        #endregion
    }
}
