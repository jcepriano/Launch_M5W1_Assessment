using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RecordCollection.DataAccess;

namespace RecordCollection.Controllers
{
    [Route("api/albums")]
    [ApiController]
    public class AlbumsAPIController : ControllerBase
    {
        private readonly RecordCollectionContext _context;
        private readonly Serilog.ILogger _logger;

        public AlbumsAPIController(RecordCollectionContext context, Serilog.ILogger logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult GetAll()
        {
            var albums = _context.Albums.ToList();
            if(albums.Any())
            {
                _logger.Information("Albums returned success"); // Logging
            }
            else
            {
                _logger.Information("No albums returned success"); // Logging
            }
            return new JsonResult(albums);
        }

        [HttpGet("{id}")]
        public IActionResult GetOne(int id)
        {
            var album = _context.Albums.FirstOrDefault(a => a.Id == id);
            if(album != null)
            {
                _logger.Information("Single album returned success"); // Logging
            }
            else
            {
                _logger.Information("No single album returned success"); // Logging
            }

            return new JsonResult(album);
        }

        [HttpDelete("{id}")]
        public void DeleteOne(int id)
        {
            var album = _context.Albums.FirstOrDefault(a => a.Id == id);
            _context.Albums.Remove(album);
            _context.SaveChanges();
        }
    }
}
