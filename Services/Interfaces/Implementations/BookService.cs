using LibraryAPI.Data;
using LibraryAPI.Models;
using LibraryAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Services.Implementations
{
    public class BookService : IBookService
    {
        private readonly LibraryContext _context;
        public BookService(LibraryContext context) => _context = context;

        public IEnumerable<Book> GetAll() => _context.Books
            .Include(book => book.Library)
                .ThenInclude(library => library!.Books)
            .Include(book => book.Library)
                .ThenInclude(library => library!.Members)
            .Include(book => book.Loans)
            .AsSplitQuery()
            .ToList();
        public Book? GetById(int id) => _context.Books
            .Include(book => book.Library)
                .ThenInclude(library => library!.Books)
            .Include(book => book.Library)
                .ThenInclude(library => library!.Members)
            .Include(book => book.Loans)
            .AsSplitQuery()
            .FirstOrDefault(book => book.Id == id);

        public void Add(Book book)
        {
            _context.Books.Add(book);
            _context.SaveChanges();
        }

        public void Update(Book book)
        {
            _context.Books.Update(book);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var book = _context.Books.Find(id);
            if (book != null)
            {
                _context.Books.Remove(book);
                _context.SaveChanges();
            }
        }
    }
}
