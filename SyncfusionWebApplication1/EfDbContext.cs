using Microsoft.EntityFrameworkCore;
using SyncfusionWebApplication1.Data;

namespace SyncfusionWebApplication1
{
    public class EfDbContext : DbContext
    {
        public EfDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Test> Tests { get; set; }
    }
}
