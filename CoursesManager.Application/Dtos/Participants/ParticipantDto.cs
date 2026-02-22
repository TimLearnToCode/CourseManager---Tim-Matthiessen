namespace CoursesManager.Application.Dtos.Participants;

public record ParticipantDto(int Id, string FirstName, string LastName, string Email, DateTime CreatedAt, DateTime UpdatedAt);
