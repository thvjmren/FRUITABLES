using Fruitables.Models;

namespace Fruitables.ViewModels
{
    public class UpdateProductVM
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public int? CategoryId { get; set; }
        public string PrimaryImage { get; set; }
        public IFormFile? mainPhoto { get; set; }
        public List<Category> Categories { get; set; }
    }
}
