using CoursesManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoursesManager.Infrastructure.Persistence.Configurations;

public class RegistrationConfiguration : IEntityTypeConfiguration<RegistrationEntity>
{
    public void Configure(EntityTypeBuilder<RegistrationEntity> builder)
    {
        builder.HasKey(e => e.Id);

        //Ser till att samma deltagare inte kan registreras två gånger på samma kurstillfälle.
        builder.HasIndex(e => new { e.ParticipantId, e.CourseSessionId })
            .IsUnique();

        builder.Property(e => e.RegisteredAt)
            .HasColumnType("datetime2(0)")
            .HasDefaultValueSql("SYSUTCDATETIME()");

        builder.Property(e => e.CreatedAt)
            .HasColumnType("datetime2(0)")
            .HasDefaultValueSql("SYSUTCDATETIME()");

        builder.Property(e => e.UpdatedAt)
            .HasColumnType("datetime2(0)")
            .HasDefaultValueSql("SYSUTCDATETIME()");

        builder.HasOne(r => r.Participant)
            .WithMany(p => p.Registrations)
            .HasForeignKey(r => r.ParticipantId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(r => r.CourseSession)
            .WithMany(cs => cs.Registrations)
            .HasForeignKey(r => r.CourseSessionId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.ToTable("Registrations");
    }
}
