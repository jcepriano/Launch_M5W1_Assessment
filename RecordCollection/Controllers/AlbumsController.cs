using Microsoft.AspNetCore.Mvc;
using RecordCollection.DataAccess;
using RecordCollection.Models;

namespace RecordCollection.Controllers
{
    public class AlbumsController : Controller
    {

        private readonly RecordCollectionContext _context;
        private readonly Serilog.ILogger _logger;

        public AlbumsController(RecordCollectionContext context, Serilog.ILogger logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            var albums = _context.Albums.ToList();
            if (albums != null) _logger.Information("Albums returned in view"); // Logging

            return View(albums);
        }

        [Route("/albums/{id:int}")]
        public IActionResult Show(int? id)
        {
            if (id == null) return BadRequest(); // Null Checking

            var album = _context.Albums.FirstOrDefault(a => a.Id == id);

            if (album == null) return NotFound(); // Null Checking

            return View(album);
        }

        public IActionResult New()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Album album)
        {
            if (ModelState.IsValid) // Model Validation
            {
                _context.Albums.Add(album);
                _context.SaveChanges();

                _logger.Information("this is the create action");

                return RedirectToAction(nameof(Index));
            }
            else
            {
                _logger.Warning("user failed to enter all fields"); // Logging
                return View("New", album);
            }

        }

        [HttpPost]
        [Route("/albums/{id:int}")]
        public IActionResult Delete(int? id)
        {
            var album = _context.Albums.FirstOrDefault(a => a.Id == id);
            _context.Albums.Remove(album);
            _context.SaveChanges();

            _logger.Fatal($"Success! {album.Title} was removed from the database.");

            return RedirectToAction(nameof(Index));
        }
    }
}
