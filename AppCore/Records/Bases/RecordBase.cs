using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AppCore.Records.Bases
{
    public abstract class RecordBase
    {
        public int Id { get; set; }
        public string Guid { get; set; }
        public DateTime? OlusturmaTarih { get; set; }
        public int? OlusturanKullaniciId { get; set; }
        public DateTime? GuncellemeTarih { get; set; }
        public int? GuncelleyenKullaniciId { get; set; }
        [DisplayName("Aktif Mi")]
        public bool AktifMi { get; set; }
    }
}
