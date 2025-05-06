using Fruitables.Models;
using Microsoft.AspNetCore.Mvc;

namespace Fruitables.ViewModels
{
    public class HomeVM 
    {
        public List<Product> Products { get; set; }
        public List<Slide> Slides { get; set; }
    }
}
