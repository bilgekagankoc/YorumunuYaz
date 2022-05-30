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
    public class KategoriModel : RecordBase
    {
        #region Entityden alınanlar
        [Required(ErrorMessage = "{0} gereklidir!")]
        [MinLength(1, ErrorMessage = "{0} en az {1} karakter olmalıdır!")]
        [MaxLength(22, ErrorMessage = "{0} en çok {1} karakter olmalıdır!")]
        [DisplayName("Kategori Adı")]
        public string Ad { get; set; }
        [DisplayName("Kullanıcı Açıklaması")]
        public string Aciklama { get; set; }
        public List<Yorum> Yorumlar { get; set; }
        #endregion
        #region Sayfanın İhtiyacı
        [DisplayName("Yorum Sayısı")]
        public int YorumSayısı { get; set; }
        #endregion
    }
}
