using Microsoft.EntityFrameworkCore;

namespace RoboSchoolBDProjectBackend.Models
{
    public class ManagerContext : DbContext
     {
        public DbSet<Manager> Managers { get; set; }

        public ManagerContext(DbContextOptions<ManagerContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
    
}
