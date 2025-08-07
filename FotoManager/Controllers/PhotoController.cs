using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FotoManager.Data;
using FotoManager.Models;


namespace FotoManager.Controllers
{
    public class PhotoController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public PhotoController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public async Task<IActionResult> Index(string filter, int? bookId)
        {
            var photos = _context.Photos.Include(p => p.Book).AsQueryable();

            if (!string.IsNullOrEmpty(filter))
                photos = photos.Where(p => p.Filter.Contains(filter) || p.Frame.Contains(filter));

            if (bookId.HasValue)
                photos = photos.Where(p => p.BookId == bookId);

            ViewBag.Books = await _context.Books.ToListAsync();
            return View(await photos.ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Books = await _context.Books.ToListAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Photo photo, IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                var path = Path.Combine(_environment.WebRootPath, "uploads", "photos", fileName);

                Directory.CreateDirectory(Path.GetDirectoryName(path)!);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                photo.FileName = fileName;
                photo.UploadDate = DateTime.Now;

                _context.Photos.Add(photo);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}