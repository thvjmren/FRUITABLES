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
    public class SlideController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public SlideController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            List<GetSlideVM> slideVMs = await _context.Slides.Select(s=>

            new GetSlideVM
            {
                Id = s.Id,
                Title = s.Title,
                Subtitle = s.Subtitle,
                Image=s.Image,
                Order = s.Order,
                CreatedAT = s.CreatedTime
            }).ToListAsync();

            return View(slideVMs);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> Create(CreateSlideVM slideVM)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (!slideVM.Photo.ContentType.Contains("image/"))
            {
                ModelState.AddModelError(nameof(CreateSlideVM.Photo), "file type is incorrect");
                return View();
            }

            if (slideVM.Photo.Length > 1 * 1024 * 1024)
            {
                ModelState.AddModelError(nameof(CreateSlideVM.Photo), "file size should be less than 1 MB");
                return View();
            }

            bool exists = await _context.Slides.AnyAsync(s => s.Order == slideVM.Order);

            if (exists)
            {
                ModelState.AddModelError("order", "this order number is already exists");
                return View(slideVM);
            }


            string fileName = string.Concat(Guid.NewGuid().ToString(), slideVM.Photo.FileName.Substring(slideVM.Photo.FileName.LastIndexOf('.')));

            string path = Path.Combine(_env.WebRootPath, "img", fileName);
            FileStream fl = new FileStream(path, FileMode.Create);
            await slideVM.Photo.CopyToAsync(fl);
            fl.Close();

            Slide slide = new()
            {
                Title = slideVM.Title,
                Subtitle = slideVM.Subtitle,
                Order = slideVM.Order,
                Description = slideVM.Description,
                Image = fileName,
                CreatedTime = DateTime.Now,
            };


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


        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null || id <= 0) return BadRequest();

            Slide? slide = await _context.Slides
                .Where(c => c.IsDeleted == false)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (slide is null) return NotFound();

            _context.Slides.Remove(slide);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id is null || id <= 0) return BadRequest();

            Slide? slide = await _context.Slides.FirstOrDefaultAsync(s => s.Id == id);

            if (slide is null) return NotFound();

            UpdateSlideVM updateSlideVM = new()
            {
                Title = slide.Title,
                Subtitle = slide.Subtitle,
                Description = slide.Description,
                Order = slide.Order,
                Image = slide.Image
            };
            return View(updateSlideVM);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int? id, UpdateSlideVM slideVM)
        {
            if (!ModelState.IsValid) return BadRequest();

            Slide? existed = await _context.Slides.FirstOrDefaultAsync(s => s.Id == id);

            if (existed is null) return NotFound();

            bool exists = await _context.Slides.AnyAsync(s => s.Order == slideVM.Order);
            bool existName = await _context.Slides.AnyAsync(s => s.Title == slideVM.Title);

            if (exists)
            {
                ModelState.AddModelError(nameof(UpdateSlideVM.Order), "this order number is already taken");
                return View(slideVM);
            }

            if (slideVM.Photo is not null)
            {
                if (!slideVM.Photo.ValidateSize(FileSize.MB, 5))
                {
                    ModelState.AddModelError(nameof(UpdateSlideVM.Photo), "only 5 mb");
                    return View(slideVM);
                }

                if (!slideVM.Photo.ValidateType("image/"))
                {
                    ModelState.AddModelError(nameof(UpdateSlideVM.Photo), "only image");
                    return View(slideVM);
                }
                string fileName = await slideVM.Photo.CreateFileAsync(_env.WebRootPath, "img");
                existed.Image.DeleteFile(_env.WebRootPath, "img");
                existed.Image = fileName;

            }
            existed.Title = slideVM.Title;
            existed.Description = slideVM.Description;
            existed.Subtitle = slideVM.Subtitle;
            existed.Order = slideVM.Order;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}