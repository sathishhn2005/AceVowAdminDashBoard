using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AceVowEntities
{
    public class PreviewDeals
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string image { get; set; }
        public Nullable<decimal> RegularPrice { get; set; }
        public decimal OfferPrice { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public int UserId { get; set; }
        public Nullable<bool> IsRecommended { get; set; }
        public string TaxType { get; set; }
        public string Unit { get; set; }
        public string Quantity { get; set; }
        public decimal SavingPrice { get; set; }
        public decimal SavingPercentage { get; set; }
        public decimal Price { get; set; }
        public string ParentCategory { get; set; }
        public int CategoryId { get; set; }
        public int ClientLogo { get; set; }
    }
}
