namespace CoursesManager.Domain.Entities;

public class CourseSessionEntity
{
    public int Id { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int MaxParticipants { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }


    public int CourseId { get; set; }
    public CourseEntity Course { get; set; } = null!;

    public int LocationId { get; set; }
    public LocationEntity Location { get; set; } = null!;

    public int TeacherId { get; set; }
    public TeacherEntity Teacher { get; set; } = null!;

    public ICollection<RegistrationEntity> Registrations { get; set; } = [];
}