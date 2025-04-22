using Microsoft.EntityFrameworkCore;

namespace apigw_monitor.Data
{
    public class ePingDBContext : DbContext
    {
        public ePingDBContext(DbContextOptions<ePingDBContext> options) : base(options) { }
    }
}