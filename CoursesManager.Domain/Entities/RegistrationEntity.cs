namespace CoursesManager.Domain.Entities;

public class RegistrationEntity
{
    public int Id { get; set; }
    public DateTime RegisteredAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public int ParticipantId { get; set; }
    public ParticipantEntity Participant { get; set; } = null!;

    public int CourseSessionId { get; set; }
    public CourseSessionEntity CourseSession { get; set; } = null!;
}
