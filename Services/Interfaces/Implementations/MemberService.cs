using LibraryAPI.Data;
using LibraryAPI.Models;
using LibraryAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Services.Implementations
{
    public class MemberService : IMemberService
    {
        private readonly LibraryContext _context;
        public MemberService(LibraryContext context) => _context = context;

        public IEnumerable<Member> GetAll() => _context.Members
            .Include(member => member.Library)
                .ThenInclude(library => library!.Books)
            .Include(member => member.Library)
                .ThenInclude(library => library!.Members)
            .Include(member => member.Loans)
            .AsSplitQuery()
            .ToList();

        public Member? GetById(int id) => _context.Members
            .Include(member => member.Library)
                .ThenInclude(library => library!.Books)
            .Include(member => member.Library)
                .ThenInclude(library => library!.Members)
            .Include(member => member.Loans)
            .AsSplitQuery()
            .FirstOrDefault(member => member.Id == id);

        public void Add(Member member)
        {
            _context.Members.Add(member);
            _context.SaveChanges();
        }

        public void Update(Member member)
        {
            _context.Members.Update(member);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var member = _context.Members.Find(id);
            if (member != null)
            {
                _context.Members.Remove(member);
                _context.SaveChanges();
            }
        }
    }
}
