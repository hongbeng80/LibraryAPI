using Xunit;
using Microsoft.EntityFrameworkCore;
using LibraryAPI.Data;
using LibraryAPI.Models;
using LibraryAPI.Services.Implementations;

namespace LibraryAPI.Tests
{
    public class LoanServiceTests
    {
        [Fact]
        public void BorrowBook_ShouldMarkBookUnavailable()
        {
            var options = new DbContextOptionsBuilder<LibraryContext>()
                .UseInMemoryDatabase("LoanDb").Options;

            using var context = new LibraryContext(options);
            var service = new LoanService(context);

            var book = new Book { Title = "Loan Book", Author = "Author", IsAvailable = true };
            var member = new Member { Name = "Jane Doe", Email = "jane@example.com" };
            context.Books.Add(book);
            context.Members.Add(member);
            context.SaveChanges();

            var loan = service.BorrowBook(book.Id, member.Id);

            Assert.False(context.Books.Find(book.Id)!.IsAvailable);
            Assert.NotNull(loan);
        }

        [Fact]
        public void ReturnBook_ShouldMarkBookAvailable()
        {
            var options = new DbContextOptionsBuilder<LibraryContext>()
                .UseInMemoryDatabase("LoanDb2").Options;

            using var context = new LibraryContext(options);
            var service = new LoanService(context);

            var book = new Book { Title = "Return Book", Author = "Author", IsAvailable = true };
            var member = new Member { Name = "Jane Doe", Email = "jane@example.com" };
            context.Books.Add(book);
            context.Members.Add(member);
            context.SaveChanges();

            var loan = service.BorrowBook(book.Id, member.Id);
            service.ReturnBook(loan.Id);

            Assert.True(context.Books.Find(book.Id)!.IsAvailable);
            Assert.NotNull(context.Loans.Find(loan.Id)!.ReturnDate);
        }
    }
}
