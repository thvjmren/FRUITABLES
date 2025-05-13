using Fruitables.Data;
using Fruitables.Models;
using Fruitables.Utilities.Enums;
using Fruitables.Utilities.Extensions;
using Fruitables.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fruitables.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ProductController(AppDbContext context,IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            List<GetProductVM> productVMs = await _context.Products.Select(p =>

            new GetProductVM
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                CategoryName = p.Category.Name,
                MainImage = p.Images.FirstOrDefault(p => p.IsPrimary == true).Image
            }).ToListAsync();

            return View(productVMs);
        }

        public async Task<IActionResult> Create()
        {
            CreateProductVM productVM = new CreateProductVM
            {
                Categories = await _context.Categories.ToListAsync(),
            };
            return View(productVM);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateProductVM productVM)
        {
            productVM.Categories = await _context.Categories.ToListAsync();

            if (!ModelState.IsValid)
            {
                return View(productVM);
            }

            bool result = productVM.Categories.Any(c => c.Id == productVM.CategoryId);
            if (!result)
            {
                ModelState.AddModelError(nameof(CreateProductVM.CategoryId), "category does not exist");
                return View(productVM);
            }

            if (!productVM.mainPhoto.ValidateType("image/"))
            {
                ModelState.AddModelError(nameof(CreateProductVM.mainPhoto), "only image");
                return View(productVM);
            }

            if (!productVM.mainPhoto.ValidateSize(FileSize.MB, 5))
            {
                ModelState.AddModelError(nameof(CreateProductVM.mainPhoto), "photo size should be less than 5MB");
                return View(productVM);
            }

            bool nameResult = await _context.Products.AnyAsync(p => p.Name == productVM.Name);
            if (nameResult)
            {
                ModelState.AddModelError(nameof(CreateProductVM.Name), "this name is already exist");
                return View(productVM);
            }

            ProductImage mainImage = new ProductImage
            {
                Image = await productVM.mainPhoto.CreateFileAsync(_env.WebRootPath, "img"),
                IsPrimary = true,
                CreatedTime = DateTime.Now,
            };

            Product product = new Product
            {
                Name = productVM.Name,
                Price = productVM.Price.Value,
                CategoryId = productVM.CategoryId,
                Description = productVM.Description,
                Images = new List<ProductImage> { mainImage }
            };

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id is null&&id<=0) return BadRequest();

            Product product = await _context.Products.FirstOrDefaultAsync(p=>p.Id== id);

            if (product is null) return NotFound();

        }

    }
}
