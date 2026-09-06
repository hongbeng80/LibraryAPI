using LibraryAPI.Models;

namespace LibraryAPI.Services.Interfaces
{
    public interface IMemberService
    {
        IEnumerable<Member> GetAll();
        Member? GetById(int id);
        void Add(Member member);
        void Update(Member member);
        void Delete(int id);
    }
}
