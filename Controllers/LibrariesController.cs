using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LibraryAPI.Data;
using LibraryAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = "LibraryStaff")]
    public class LibrariesController : ControllerBase
    {
        private readonly LibraryContext _context;
        public LibrariesController(LibraryContext context) => _context = context;

        [HttpGet]
        public IActionResult GetAll() => Ok(_context.Libraries
            .Include(library => library.Books)
            .Include(library => library.Members)
            .ToList());

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var library = _context.Libraries
                .Include(library => library.Books)
                .Include(library => library.Members)
                .FirstOrDefault(library => library.Id == id);
            return library == null ? NotFound() : Ok(library);
        }

        [HttpPost]
        public IActionResult Add(LibraryRequest request)
        {
            var library = new Library { Name = request.Name, Address = request.Address };
            _context.Libraries.Add(library);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetById), new { id = library.Id }, library);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, LibraryRequest request)
        {
            var library = _context.Libraries.Find(id);
            if (library == null) return NotFound();
            library.Name = request.Name;
            library.Address = request.Address;
            _context.Libraries.Update(library);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var library = _context.Libraries.Find(id);
            if (library == null) return NotFound();
            _context.Libraries.Remove(library);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
