using AppCore.Records.Bases;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    public class YorumCevap : RecordBase
    {
        [Required]
        [MinLength(1)]
        [MaxLength(500)]
        public string Cevap { get; set; }
        public int KullaniciId { get; set; }
        public Kullanici Kullanici { get; set; }
        public int YorumId { get; set; }
        public Yorum Yorum { get; set; }
    }
}
