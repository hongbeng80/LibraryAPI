using LibraryAPI.Data;
using LibraryAPI.Models;

namespace LibraryAPI.Middleware
{
    public class AuditMiddleware
    {
        private readonly RequestDelegate _next;

        public AuditMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, LibraryContext db)
        {
            // Capture request details
            var log = new AuditLog
            {
                Action = $"{context.Request.Method} {context.Request.Path}",
                User = context.User.Identity?.Name ?? "Anonymous",
                Timestamp = DateTime.UtcNow
            };

            db.AuditLogs.Add(log);
            await db.SaveChangesAsync();

            // Continue pipeline
            await _next(context);
        }
    }

    // Extension method for easy registration
    public static class AuditMiddlewareExtensions
    {
        public static IApplicationBuilder UseAuditLogging(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuditMiddleware>();
        }
    }
}
