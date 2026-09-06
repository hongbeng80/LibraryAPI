using Xunit;
using Microsoft.EntityFrameworkCore;
using LibraryAPI.Data;
using LibraryAPI.Models;
using LibraryAPI.Services.Implementations;

namespace LibraryAPI.Tests
{
    public class MemberServiceTests
    {
        [Fact]
        public void AddMember_ShouldIncreaseCount()
        {
            var options = new DbContextOptionsBuilder<LibraryContext>()
                .UseInMemoryDatabase("MemberDb").Options;

            using var context = new LibraryContext(options);
            var service = new MemberService(context);

            service.Add(new Member { Name = "John Doe", Email = "john@example.com" });

            Assert.Equal(1, context.Members.Count());
        }

        [Fact]
        public void UpdateMember_ShouldChangeName()
        {
            var options = new DbContextOptionsBuilder<LibraryContext>()
                .UseInMemoryDatabase("MemberDb2").Options;

            using var context = new LibraryContext(options);
            var service = new MemberService(context);

            var member = new Member { Name = "Old Name", Email = "old@example.com" };
            context.Members.Add(member);
            context.SaveChanges();

            member.Name = "New Name";
            service.Update(member);

            Assert.Equal("New Name", context.Members.First().Name);
        }
    }
}
