using Microsoft.EntityFrameworkCore;
using RoboSchoolBDProjectBackend.Models.Admin;
using RoboSchoolBDProjectBackend.Models.OutObjects;
using RoboSchoolBDProjectBackend.Models.OutObjects.ComplexObjDB;

namespace RoboSchoolBDProjectBackend.Models
{
    public class AdminContext : DbContext
     {
        public DbSet<ManagerOut> ManagersOut { get; set; }

        public DbSet<School_items> School_items { get; set; }
        public DbSet<Course_items> Course_items { get; set; }
        public DbSet<Request_items> Request_items { get; set; }

        public DbSet<TeacherOut> TeachersOut { get; set; }
        public DbSet<ProviderOut> ProvidersOut { get; set; }
        public DbSet<ItemOut> ItemsOut { get; set; }
        
        public DbSet<HashSalt> HashSalts { get; set; }

        public AdminContext(DbContextOptions<AdminContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
    
}
