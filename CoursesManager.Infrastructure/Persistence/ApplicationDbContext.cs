using CoursesManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CoursesManager.Infrastructure.Persistence;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<CourseEntity> Courses { get; set; }
    public DbSet<CourseSessionEntity> CourseSessions { get; set; }
    public DbSet<LocationEntity> Locations { get; set; }
    public DbSet<TeacherEntity> Teachers { get; set; }
    public DbSet<ParticipantEntity> Participants { get; set; }
    public DbSet<RegistrationEntity> Registrations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Hämtar automatiskt alla konfigurationsfiler som bestämmer hur tabellerna ska se ut i databasen.
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}
