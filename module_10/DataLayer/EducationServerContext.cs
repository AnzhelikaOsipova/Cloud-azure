using Microsoft.EntityFrameworkCore;
using Models.Database;
using System.Linq;

namespace DataLayer
{
    public class EducationServerContext: DbContext
    {
        public EducationServerContext(DbContextOptions options) :
            base(options)
        {
            
            if (Database.IsRelational() && Database.GetMigrations().Any())
            {
                Database.Migrate();
            }
            else
            {
                Database.EnsureCreated();
            }
        }

        public void ReCreateDatabase()
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        public DbSet<Lector> Lectors { get; set; }
        public DbSet<Lection> Lections { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Homework> Homeworks { get; set; }
        public DbSet<Attendance> Attendance { get; set; }
    }
}
