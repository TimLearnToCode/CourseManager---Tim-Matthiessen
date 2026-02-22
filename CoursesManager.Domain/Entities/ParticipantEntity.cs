namespace CoursesManager.Domain.Entities;

public class ParticipantEntity
{
    public int Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public ICollection<RegistrationEntity> Registrations { get; set; } = [];
}
