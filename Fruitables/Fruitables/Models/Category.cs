using Fruitables.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace Fruitables.Models
{
    public class Category:BaseEntity
    {
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "only letters")]
        [Required]
        [MaxLength(100)]
        [MinLength(5)]
        public string Name { get; set; }
        public List<Product>? Products { get; set; }
    }
}
