using Fruitables.Models.Base;

namespace Fruitables.Models
{
    public class Slide:BaseEntity
    {
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string Image { get; set; }
        public int Order { get; set; }
    }
}
