namespace CoursesManager.Application.Dtos.Teachers;

public record TeacherDto(int Id, string FirstName, string LastName, string Email, DateTime CreatedAt, DateTime UpdatedAt);
