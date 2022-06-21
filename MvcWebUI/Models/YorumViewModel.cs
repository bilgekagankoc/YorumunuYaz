using Business.Models;

namespace MvcWebUI.Models
{
    public class YorumViewModel
    {
        public List<YorumModel> YorumModels { get; set; }
        public YorumModel YorumModel { get; set; }
        public YorumCevapModel YorumCevap { get; set; }
        public List<YorumCevapModel> YorumCevaplar { get; set; }
        public string MevcutKategori { get; set; }
    }
}
