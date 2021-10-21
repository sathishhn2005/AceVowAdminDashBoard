using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AceVowEntities
{
    public class Recipes
    {
        public long RecipeId { get; set; }
        public Nullable<int> ProductId { get; set; }
        public Nullable<int> UserId { get; set; }
        public Nullable<int> CategoryId { get; set; }
        public string Ingredients { get; set; }
        public string ProductName { get; set; }
        public string RecipeCategory { get; set; }
        public string Credits { get; set; }
        public string Serving { get; set; }
        public bool IsActive { get; set; }

        public string CategoryName { get; set; }
        public string UserName { get; set; }
        public string RecipeName { get; set; }
        public string ImageName { get; set; }
        public string Quantity { get; set; }
        public string Duration { get; set; }
    }
}
