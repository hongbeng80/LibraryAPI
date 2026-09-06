using Xunit;
using Microsoft.EntityFrameworkCore;
using LibraryAPI.Data;
using LibraryAPI.Models;
using LibraryAPI.Services.Implementations;

namespace LibraryAPI.Tests
{
    public class BookServiceTests
    {
        [Fact]
        public void AddBook_ShouldIncreaseCount()
        {
            var options = new DbContextOptionsBuilder<LibraryContext>()
                .UseInMemoryDatabase("BookDb").Options;

            using var context = new LibraryContext(options);
            var service = new BookService(context);

            service.Add(new Book { Title = "Test Book", Author = "Author" });

            Assert.Equal(1, context.Books.Count());
        }

        [Fact]
        public void DeleteBook_ShouldRemoveBook()
        {
            var options = new DbContextOptionsBuilder<LibraryContext>()
                .UseInMemoryDatabase("BookDb2").Options;

            using var context = new LibraryContext(options);
            var service = new BookService(context);

            var book = new Book { Title = "Delete Me", Author = "Author" };
            context.Books.Add(book);
            context.SaveChanges();

            service.Delete(book.Id);

            Assert.Empty(context.Books);
        }
    }
}
