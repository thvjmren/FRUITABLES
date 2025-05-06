using Fruitables.Data;
using Fruitables.Models;
using Fruitables.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fruitables.Controllers
{
    public class ShopController : Controller
    {
        private readonly AppDbContext _context;

        public ShopController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Detail(int? id)
        {
            if (id is null || id <= 0)
            {
                return BadRequest();
            }
            Product? product = _context.Products
                .Include(p => p.Images)
                .Include(p=>p.Category)
                .FirstOrDefault(p => p.Id == id);

            if (product is null) return NotFound();

            ShopVM shopVM = new ShopVM()
            {
                Product = product,
                RelatedProduct = _context.Products
                .Include(p => p.Images)
                .Include(p => p.Category)
                .ToList(),
            };

            return View(shopVM);

        }
    }
}
