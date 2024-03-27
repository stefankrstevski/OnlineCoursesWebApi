using OnlineCoursesWebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace OnlineCoursesWebApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Course> Courses { get; set; }
        public DbSet<CourseDate> CourseDates { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Participant> Participants { get; set; }
        public DbSet<Application> Applications { get; set; }
        public DbSet<ApplicationParticipant> ApplicationParticipants { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationParticipant>()
                .HasKey(ap => new { ap.ApplicationId, ap.ParticipantId });

            modelBuilder.Entity<ApplicationParticipant>()
                .HasOne(ap => ap.Application)
                .WithMany(a => a.ApplicationParticipants)
                .HasForeignKey(ap => ap.ApplicationId);

            modelBuilder.Entity<ApplicationParticipant>()
                .HasOne(ap => ap.Participant)
                .WithMany(p => p.ApplicationParticipants)
                .HasForeignKey(ap => ap.ParticipantId);
        }
    }
}
