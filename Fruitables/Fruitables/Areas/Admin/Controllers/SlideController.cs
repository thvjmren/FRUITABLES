using Fruitables.Data;
using Fruitables.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Fruitables.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SlideController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public SlideController(AppDbContext context,IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            List<Slide> slides = await _context.Slides.ToListAsync();

            return View(slides);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> Create(Slide slide)
        {
            if (!slide.Photo.ContentType.Contains("image/"))
            {
                ModelState.AddModelError(nameof(Slide.Photo), "file type is incorrect");
                return View();
            }

            if (slide.Photo.Length > 1 * 1024 * 1024)
            {
                ModelState.AddModelError(nameof(Slide.Photo), "file size should be less than 1 MB");
                return View();
            }

            bool exists = await _context.Slides.AnyAsync(s=>s.Order == slide.Order);

            if(exists)
            {
                ModelState.AddModelError("order", "this order number is already exists");
                return View(slide);
            }

            string fileName = string.Concat(Guid.NewGuid().ToString(), slide.Photo.FileName.Substring(slide.Photo.FileName.LastIndexOf('.')));

            string path = Path.Combine(_env.WebRootPath,"img",fileName);
            FileStream fl = new FileStream(path, FileMode.Create);
            await slide.Photo.CopyToAsync(fl);

            slide.Image = slide.Photo.FileName;

            slide.CreatedTime = DateTime.Now;

            await _context.Slides.AddAsync(slide);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));


            //if(!ModelState.IsValid)
            //{ 
            //    return View();
            //}

            //bool result = await _context.Slides.AnyAsync(s=>s.Title == slide.Title);

            //if (result)
            //{
            //    ModelState.AddModelError(nameof(Slide.Title),$"{slide.Title} is already exists");
            //}
        }
    }
}

