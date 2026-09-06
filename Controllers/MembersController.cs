using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LibraryAPI.Models;
using LibraryAPI.Services.Interfaces;

namespace LibraryAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = "LibraryStaff")]
    public class MembersController : ControllerBase
    {
        private readonly IMemberService _memberService;
        public MembersController(IMemberService memberService) => _memberService = memberService;

        [HttpGet]
        public IActionResult GetAll() => Ok(_memberService.GetAll());

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var member = _memberService.GetById(id);
            return member == null ? NotFound() : Ok(member);
        }

        [HttpPost]
        public IActionResult Add(MemberRequest request)
        {
            var member = new Member
            {
                Name = request.Name,
                Email = request.Email,
                LibraryId = request.LibraryId
            };
            _memberService.Add(member);
            return CreatedAtAction(nameof(GetById), new { id = member.Id }, member);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, MemberRequest request)
        {
            var member = _memberService.GetById(id);
            if (member == null) return NotFound();
            member.Name = request.Name;
            member.Email = request.Email;
            member.LibraryId = request.LibraryId;
            _memberService.Update(member);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _memberService.Delete(id);
            return NoContent();
        }
    }
}
