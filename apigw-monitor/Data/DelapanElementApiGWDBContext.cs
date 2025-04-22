using apigw_monitor.Model;
using Microsoft.EntityFrameworkCore;

namespace apigw_monitor.Data
{
    public class DelapanElementApiGWDBContext : DbContext
    {
        public DelapanElementApiGWDBContext(DbContextOptions<DelapanElementApiGWDBContext> options) : base(options) { }
        public DbSet<MT> MT { get; set; }
    }
}