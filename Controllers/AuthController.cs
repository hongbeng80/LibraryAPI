using Microsoft.AspNetCore.Mvc;
using LibraryAPI.Models;
using LibraryAPI.Security;

namespace LibraryAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly TokenService _tokenService;

        public AuthController(TokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public IActionResult Login(string username, string password)
        {
            // For demo purposes only: replace with proper user validation
            if (username == "admin" && password == "password")
            {
                var token = _tokenService.GenerateToken(username, "Admin");
                Response.Cookies.Append("access_token", token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = Request.IsHttps,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTimeOffset.UtcNow.AddMinutes(60)
                });

                return Ok(new { Token = token });
            }
            return Unauthorized();
        }

        
    }
}
