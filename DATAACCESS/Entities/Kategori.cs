using AppCore.Records.Bases;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    public class Kategori : RecordBase
    {
        [Required]
        [MinLength(1)]
        [MaxLength(22)]
        public string Ad { get; set; }
        public string Aciklama { get; set; }
        public List<Yorum> Yorumlar { get; set; }
    }
}
