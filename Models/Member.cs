namespace LibraryAPI.Models
{
    public class Member
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime MembershipDate { get; set; } = DateTime.UtcNow;

        // Navigation
        public ICollection<Loan>? Loans { get; set; }
        public int LibraryId { get; set; }
        public Library? Library { get; set; }
    }
}
