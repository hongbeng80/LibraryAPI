using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LibraryAPI.Services.Interfaces;

namespace LibraryAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = "LibraryStaff")]
    public class LoansController : ControllerBase
    {
        private readonly ILoanService _loanService;
        public LoansController(ILoanService loanService) => _loanService = loanService;

        [HttpGet("member/{memberId}")]
        public IActionResult GetLoansByMember(int memberId) =>
            Ok(_loanService.GetLoansByMember(memberId));

        [HttpPost]
        public IActionResult BorrowBook([FromQuery] int bookId, [FromQuery] int memberId)
        {
            try
            {
                return Ok(_loanService.BorrowBook(bookId, memberId));
            }
            catch (ArgumentException exception)
            {
                return BadRequest(new { error = exception.Message });
            }
            catch (InvalidOperationException exception)
            {
                return Conflict(new { error = exception.Message });
            }
        }

        [HttpPut("{loanId}/return")]
        public IActionResult ReturnBook(int loanId)
        {
            try
            {
                _loanService.ReturnBook(loanId);
                return NoContent();
            }
            catch (KeyNotFoundException exception)
            {
                return NotFound(new { error = exception.Message });
            }
            catch (InvalidOperationException exception)
            {
                return Conflict(new { error = exception.Message });
            }
        }
    }
}
