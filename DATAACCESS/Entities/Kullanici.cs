using AppCore.Records.Bases;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    public class Kullanici : RecordBase
    {
        [Required]
        [MinLength(3)]
        [MaxLength(15)]
        public string KullaniciAdi { get; set; }

        [Required]
        [StringLength(10)] 
        public string Sifre { get; set; }
        [Required]
        public string ePosta { get; set; }

        public bool AktifMi { get; set; }
        public int RolId { get; set; }
        public Rol Rol { get; set; }
    }
}
