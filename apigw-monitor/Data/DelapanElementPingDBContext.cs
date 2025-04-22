using apigw_monitor.Model;
using Microsoft.EntityFrameworkCore;

namespace apigw_monitor.Data
{
    public class DelapanElementPingDBContext : DbContext
    {
        public DelapanElementPingDBContext(DbContextOptions<DelapanElementPingDBContext> options) : base(options) { }
        public DbSet<RequestLog> RequestLogs { get; set; }
    }
}
