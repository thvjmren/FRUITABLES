using Fruitables.Models.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fruitables.Models
{
    public class Slide:BaseEntity
    {
        [Required]
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string? Image { get; set; }
        public int Order { get; set; }
        public string Description { get; set; }
    }
}
