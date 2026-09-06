namespace LibraryAPI.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string ISBN { get; set; } = string.Empty;
        public int PublishedYear { get; set; }
        public bool IsAvailable { get; set; } = true;

        // Navigation
        public ICollection<Loan>? Loans { get; set; }
        public int LibraryId { get; set; }
        public Library? Library { get; set; }
    }
}
