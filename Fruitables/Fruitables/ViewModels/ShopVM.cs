using Fruitables.Models;

namespace Fruitables.ViewModels
{
    public class ShopVM
    {
        public Product Product { get; set; }
        public List<Product> RelatedProduct { get; set; }
    }
}
