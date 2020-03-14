using System.Collections.Generic;
using MyShop.Core.Models;

namespace MyShop.Core.ViewModels
{
    public class ProductViewModel
    {
        public Product Product { get; set; }
        public IEnumerable<ProductCategory> ProductCategories { get; set; }
    }
}
