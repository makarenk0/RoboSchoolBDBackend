using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using RoboSchoolBDProjectBackend.DataBaseModel;

namespace RoboSchoolBDProjectBackend.Models.Teacher
{
    public class TeacherContext : DbContext
    {
        public DbSet<Teachers> Teachers { get; set; }
        public DbSet<Groups> Groups { get; set; }
        public DbSet<HashSalt> HashSalts { get; set; }
        public DbSet<Schools> Schools { get; set; }
        public DbSet<Courses> Courses { get; set; }
        public DbSet<Requests> Requests { get; set; }
        public DbSet<Providers> Providers { get; set; }
        public DbSet<Items> Items { get; set; }

        public DbSet<Teacher_phones> Teacher_phones { get; set; }
        public DbSet<School_items> School_items { get; set; }
        public DbSet<Request_items> Request_items { get; set; }

        public TeacherContext(DbContextOptions<TeacherContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course_items>()
                .HasKey(ci => new { ci.id_course_items });
            modelBuilder.Entity<Course_items>()
                .HasOne(ci => ci.Course)
                .WithMany(c => c.items)
                .HasForeignKey(ci => ci.name_course);
            modelBuilder.Entity<Course_items>()
                .HasOne(ci => ci.Item)
                .WithMany(i => i.Course_items)
                .HasForeignKey(ci => ci.id_item);

            modelBuilder.Entity<School_items>()
               .HasKey(si => new { si.id_school_items });
            modelBuilder.Entity<School_items>()
                .HasOne(si => si.School)
                .WithMany(s => s.items)
                .HasForeignKey(si => si.id_school);
            modelBuilder.Entity<School_items>()
                .HasOne(si => si.Item)
                .WithMany(i => i.School_items)
                .HasForeignKey(si => si.id_item);

            modelBuilder.Entity<Request_items>()
                .HasKey(ri => new { ri.id_request_items });
            modelBuilder.Entity<Request_items>()
                .HasOne(ri => ri.Request)
                .WithMany(r => r.items)
                .HasForeignKey(ri => ri.id_request);
            modelBuilder.Entity<Request_items>()
                .HasOne(ri => ri.Item)
                .WithMany(i => i.Request_items)
                .HasForeignKey(ri => ri.id_item);

            modelBuilder.Entity<Teacher_phones>()
               .HasKey(tph => new { tph.phone_number });
            modelBuilder.Entity<Teacher_phones>()
                .HasOne(tph => tph.teacher)
                .WithMany(t => t.phones)
                .HasForeignKey(tph => tph.id_teacher);


        }
    }
}
