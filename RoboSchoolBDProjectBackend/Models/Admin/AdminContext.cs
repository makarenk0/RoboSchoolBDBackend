using Microsoft.EntityFrameworkCore;
using RoboSchoolBDProjectBackend.Models.Admin;
using RoboSchoolBDProjectBackend.Models.OutObjects;

namespace RoboSchoolBDProjectBackend.Models
{
    public class AdminContext : DbContext
     {
        public DbSet<ManagerOut> ManagersOut { get; set; }
        public DbSet<SchoolOut> SchoolsOut { get; set; }
        public DbSet<TeacherOut> TeachersOut { get; set; }
        public DbSet<HashSalt> HashSalts { get; set; }

        public AdminContext(DbContextOptions<AdminContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
    
}
