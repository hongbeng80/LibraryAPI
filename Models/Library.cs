namespace LibraryAPI.Models
{
    public class Library
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;

        // Navigation
        public ICollection<Book> Books { get; set; } = new List<Book>();
        public ICollection<Member> Members { get; set; } = new List<Member>();
    }
}
