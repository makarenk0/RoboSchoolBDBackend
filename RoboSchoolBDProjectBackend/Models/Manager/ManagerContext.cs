using Microsoft.EntityFrameworkCore;

namespace RoboSchoolBDProjectBackend.Models
{
    public class ManagerContext : DbContext
     {
        public DbSet<ManagerOut> ManagersOut { get; set; }
        public DbSet<HashSalt> HashSalts { get; set; }

        public ManagerContext(DbContextOptions<ManagerContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
    
}
