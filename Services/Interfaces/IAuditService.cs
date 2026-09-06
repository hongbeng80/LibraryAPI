using LibraryAPI.Models;

namespace LibraryAPI.Services.Interfaces
{
    public interface IAuditService
    {
        void LogAction(string action, string user);
        IEnumerable<AuditLog> GetAllLogs();
    }
}
