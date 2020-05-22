using Microsoft.EntityFrameworkCore;
using RoboSchoolBDProjectBackend.DataBaseModel;

namespace RoboSchoolBDProjectBackend.Models
{
    public class ManagerContext : DbContext
     {
        public DbSet<Managers> Managers { get; set; }
        public DbSet<HashSalt> HashSalts { get; set; }

        public ManagerContext(DbContextOptions<ManagerContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
    
}
