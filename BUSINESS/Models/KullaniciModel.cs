using AppCore.Records.Bases;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Models
{
    public class KullaniciModel : RecordBase
    {
        #region Entity
        [Required(ErrorMessage = "{0} gereklidir!")]
        [MinLength(3, ErrorMessage = "{0} en az {1} karakter olmalıdır!")]
        [MaxLength(15, ErrorMessage = "{0} en çok {1} karakter olmalıdır!")]
        [DisplayName("Kullanıcı Adı")]
        public string KullaniciAdi { get; set; }

        [Required(ErrorMessage = "{0} gereklidir!")]
        [StringLength(10, ErrorMessage = "{0} en fazla {1} karakter olmalıdır!")]
        [DisplayName("Şifre")]
        public string Sifre { get; set; }
        [DisplayName("Aktif")]
        public bool AktifMi { get; set; }

        [DisplayName("Rol")]
        [Required(ErrorMessage = "{0} gereklidir!")]
        public int? RolId { get; set; }
        [DisplayName("E-Posta")]
        [Required(ErrorMessage = "{0} gereklidir!")]
        [EmailAddress(ErrorMessage = "Geçersiz mail adresi!")]
        public string ePosta { get; set; }
        #endregion

        #region Gösterim için ekstra özellikler

        [DisplayName("Rol")]
        public string RolAdiDisplay { get; set; }

        [DisplayName("Aktif")]
        public string AktifDisplay { get; set; } 
        #endregion
    }
}
