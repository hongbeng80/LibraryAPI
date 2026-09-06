using Xunit;
using LibraryAPI.Security;

namespace LibraryAPI.Tests
{
    public class AuthTests
    {
        [Fact]
        public void GenerateToken_ShouldReturnValidJwt()
        {
            var settings = new JwtSettings
            {
                Key = "supersecretkey123456789012345678",
                Issuer = "TestIssuer",
                Audience = "TestAudience",
                ExpiryMinutes = 30
            };

            var tokenService = new TokenService(settings);
            var token = tokenService.GenerateToken("testuser", "Admin");

            Assert.False(string.IsNullOrEmpty(token));
            Assert.Contains("eyJ", token); // Basic check for JWT format
        }
    }
}
