namespace CoursesManager.Domain.Entities;

public class CourseEntity
{
    public int CourseId { get; set; }
    public string CourseCode { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public byte[] RowVersion { get; set; } = [];

    public ICollection<CourseSessionEntity> CourseSessions { get; set; } = [];
}



