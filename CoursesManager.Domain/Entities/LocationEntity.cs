namespace CoursesManager.Domain.Entities;



public class LocationEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public ICollection<CourseSessionEntity> CourseSessions { get; set; } = [];
}