using AppCore.Records.Bases;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    public class Yorum : RecordBase
    {
        [Required]
        [MinLength(1)]
        [MaxLength(50)]
        public string Baslik { get; set; }
        [Required]
        [MinLength(1)]
        [MaxLength(500)]
        public string Icerik { get; set; }
        [StringLength(5)]
        public string ImajDosyaUzantisi { get; set; }
        public int KategoriId { get; set; }
        public Kategori Kategori { get; set; }
        public int KullaniciId { get; set; }
        public Kullanici Kullanici { get; set; }
    }
}
