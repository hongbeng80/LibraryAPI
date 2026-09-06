namespace LibraryAPI.Models
{
    public class Loan
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public int MemberId { get; set; }
        public DateTime BorrowDate { get; set; } = DateTime.UtcNow;
        public DateTime? ReturnDate { get; set; }

        // Navigation
        public Book? Book { get; set; }
        public Member? Member { get; set; }
    }
}
