using Business.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Business.Models
{
    public class KullaniciKayitModel
    {
        [Required(ErrorMessage = "{0} gereklidir!")]
        [MinLength(3, ErrorMessage = "{0} en az {1} karakter olmalıdır!")]
        [MaxLength(15, ErrorMessage = "{0} en çok {1} karakter olmalıdır!")]
        [DisplayName("Kullanıcı Adı")]
        public string KullaniciAdi { get; set; }

        [Required(ErrorMessage = "{0} gereklidir!")]
        [StringLength(10, ErrorMessage = "{0} en fazla {1} karakter olmalıdır!")]
        [DisplayName("Şifre")]
        public string Sifre { get; set; }

        [DisplayName("Şifre Onay")]
        [Required(ErrorMessage = "{0} gereklidir!")]
        [StringLength(10, ErrorMessage = "{0} en fazla {1} karakter olmalıdır!")]
        [Compare("Sifre", ErrorMessage = "{0} ile {1} aynı olmalıdır!")]
        public string SifreOnay { get; set; }

        [DisplayName("E-Posta")]
        [Required(ErrorMessage = "{0} gereklidir!")]
        [EmailAddress(ErrorMessage = "Geçersiz mail adresi!")]
        public string ePosta { get; set; }
        [DisplayName("Rol")]
        public Rol Rol { get; set; }
    }
}
