using LibraryAPI.Data;
using LibraryAPI.Models;
using LibraryAPI.Services.Interfaces;

namespace LibraryAPI.Services.Implementations
{
    public class AuditService : IAuditService
    {
        private readonly LibraryContext _context;
        public AuditService(LibraryContext context) => _context = context;

        public void LogAction(string action, string user)
        {
            var log = new AuditLog
            {
                Action = action,
                User = user,
                Timestamp = DateTime.UtcNow
            };
            _context.AuditLogs.Add(log);
            _context.SaveChanges();
        }

        public IEnumerable<AuditLog> GetAllLogs() => _context.AuditLogs.ToList();
    }
}
