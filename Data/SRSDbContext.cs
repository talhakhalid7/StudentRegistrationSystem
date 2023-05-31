using Microsoft.EntityFrameworkCore;
using StudentRegistration.Models;
namespace StudentRegistration.Data
{
    public class SRSDbContext: DbContext
    {
        public DbSet<StudentModel> Students { get; set; }
        public DbSet<CourseModel> Courses { get; set; }
        public DbSet<RegistrationModel> Registrations { get; set; }
        public SRSDbContext(DbContextOptions<SRSDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<RegistrationModel>()
            //    .HasKey(r => new { r.StudentId, r.CourseId });

            modelBuilder.Entity<RegistrationModel>()
                .HasOne(r => r.Student)
                .WithMany(s => s.Registrations)
                .HasForeignKey(r => r.StudentId);

            modelBuilder.Entity<RegistrationModel>()
                .HasOne(r => r.Course)
                .WithMany(c => c.Registrations)
                .HasForeignKey(r => r.CourseId);
        }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    var configuration = new ConfigurationBuilder()
        //        .SetBasePath(Directory.GetCurrentDirectory())
        //        .AddJsonFile("appsettings.json")
        //        .Build();

        //    var connectionString = configuration.GetConnectionString("AppDb");
        //    optionsBuilder.UseSqlServer(connectionString);
        //}
    }
}
