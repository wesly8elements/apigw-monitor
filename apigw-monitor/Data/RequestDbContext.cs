using apigw_monitor.Model;
using Microsoft.EntityFrameworkCore;

namespace apigw_monitor.Data
{
    public class RequestDbContext : DbContext
    {
        public RequestDbContext(DbContextOptions<RequestDbContext> options) : base(options) { }
        public DbSet<RequestLog> RequestLogs { get; set; }
    }
}