using Fruitables.Models;
using System.ComponentModel.DataAnnotations;

namespace Fruitables.ViewModels
{
    public class CreateProductVM
    {
        public int Id { get;set; }
        public string Name { get; set; }
        [Required]
        public decimal? Price { get; set; }
        public string Description { get; set; }
        [Required]
        public int CategoryId { get; set; }

        public IFormFile mainPhoto { get; set; }
        public List<Category>? Categories { get; set; }
    }
}
