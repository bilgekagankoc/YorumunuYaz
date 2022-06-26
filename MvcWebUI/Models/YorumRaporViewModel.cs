using AppCore.Business.Models.Ordering;
using AppCore.Business.Models.Paging;
using Business.Models;
using Business.Models.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;

namespace MvcWebUI.Models
{
    public class YorumRaporViewModel
    {
        public List<YorumRaporModel> YorumlarRaporlar { get; set; }

        public YorumRaporFilterModel YorumFilte { get; set; }

        //public SelectList KategorilerSelectList { get; set; }

        public PageModel Sayfa { get; set; }

        public SelectList SayfalarSelectList { get; set; }

        [DisplayName("Sıra")]
        public OrderModel Sira { get; set; }

        public SelectList SiraSutunBasliklariSelectList { get; set; }

        public SelectList SiraYonSelectList { get; set; }

        public MultiSelectList KategorilerMultiSelectList { get; set; }
    }
}
