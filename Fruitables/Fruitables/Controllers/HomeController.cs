using Fruitables.Data;
using Fruitables.Models;
using Fruitables.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fruitables.Controllers
{
    public class HomeController : Controller
    {
        public readonly AppDbContext _context;
        public HomeController(AppDbContext context)
        {
            _context = context;
        }


        public IActionResult Index()
        {
            Category category = new Category();

            HomeVM vm = new HomeVM()
            {
               Slides = _context.Slides.ToList(),
               Products = _context.Products.Include(m=>m.Images)
               .Take(8)
               .ToList(),
            };

            return View(vm);
        }
    }
}
