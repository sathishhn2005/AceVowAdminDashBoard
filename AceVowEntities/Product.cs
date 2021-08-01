using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AceVowEntities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public string image { get; set; }
        public string Email { get; set; }
        public Nullable<int> UserId { get; set; }
        public Nullable<decimal> RegularPrice { get; set; }
        public string TaxType { get; set; }
        public string IsRecommended { get; set; }
        public string IsActive { get; set; }
        public string Unit { get; set; }

    }
}
