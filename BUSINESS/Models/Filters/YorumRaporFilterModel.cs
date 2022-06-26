using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Models.Filters
{
    public class YorumRaporFilterModel
    {
        public int? KategoriId { get; set; }
        [StringLength(22, ErrorMessage = "{0} en fazla {1} karakter olmalıdır!")]
        [DisplayName("Yorum Başlık")]
        public string YorumBaslik { get; set; }
        [StringLength(200, ErrorMessage = "{0} en fazla {1} karakter olmalıdır!")]
        [DisplayName("Yorum İçerik")]
        public string YorumIcerik { get; set; }
        [StringLength(20, ErrorMessage = "{0} en fazla {1} karakter olmalıdır!")]
        [DisplayName("Kullanıcı Adı")]
        public string KullaniciAdi { get; set; }
        [DisplayName("Kullanıcı Adı")]
        public DateTime? Tarih { get; set; }
        [DisplayName("Kategori")]
        public List<int> KategoriIdleri { get; set; }
    }
}
