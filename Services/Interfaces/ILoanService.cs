using LibraryAPI.Models;

namespace LibraryAPI.Services.Interfaces
{
    public interface ILoanService
    {
        IEnumerable<Loan> GetLoansByMember(int memberId);
        Loan BorrowBook(int bookId, int memberId);
        void ReturnBook(int loanId);
    }
}
