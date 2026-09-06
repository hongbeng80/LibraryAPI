using LibraryAPI.Data;
using LibraryAPI.Models;
using LibraryAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Services.Implementations
{
    public class LoanService : ILoanService
    {
        private readonly LibraryContext _context;
        public LoanService(LibraryContext context) => _context = context;

        public IEnumerable<Loan> GetLoansByMember(int memberId) =>
            _context.Loans
                .Include(loan => loan.Book)
                    .ThenInclude(book => book!.Library)
                        .ThenInclude(library => library!.Books)
                .Include(loan => loan.Book)
                    .ThenInclude(book => book!.Library)
                        .ThenInclude(library => library!.Members)
                .Include(loan => loan.Member)
                    .ThenInclude(member => member!.Library)
                        .ThenInclude(library => library!.Books)
                .Include(loan => loan.Member)
                    .ThenInclude(member => member!.Library)
                        .ThenInclude(library => library!.Members)
                .AsSplitQuery()
                .Where(loan => loan.MemberId == memberId)
                .ToList();

        public Loan BorrowBook(int bookId, int memberId)
        {
            var book = _context.Books
                .Include(book => book.Library)
                .FirstOrDefault(book => book.Id == bookId);
            if (book == null)
                throw new ArgumentException("Book not found.", nameof(bookId));

            var member = _context.Members
                .Include(member => member.Library)
                .FirstOrDefault(member => member.Id == memberId);
            if (member == null)
                throw new ArgumentException("Member not found.", nameof(memberId));

            if (book.LibraryId != member.LibraryId)
                throw new ArgumentException("Book and member must belong to the same library.");

            if (!book.IsAvailable || _context.Loans.Any(loan =>
                    loan.BookId == bookId && loan.ReturnDate == null))
                throw new InvalidOperationException("Book is already on loan.");

            book.IsAvailable = false;
            var loan = new Loan
            {
                BookId = bookId,
                MemberId = memberId,
                Book = book,
                Member = member
            };
            _context.Loans.Add(loan);
            _context.SaveChanges();
            return loan;
        }

        public void ReturnBook(int loanId)
        {
            var loan = _context.Loans.Find(loanId);
            if (loan == null) throw new KeyNotFoundException("Loan not found.");
            if (loan.ReturnDate.HasValue)
                throw new InvalidOperationException("Loan has already been returned.");

            loan.ReturnDate = DateTime.UtcNow;
            var book = _context.Books.Find(loan.BookId);
            if (book == null)
                throw new InvalidOperationException("The loan's book no longer exists.");

            book.IsAvailable = true;

            _context.SaveChanges();
        }
    }
}
