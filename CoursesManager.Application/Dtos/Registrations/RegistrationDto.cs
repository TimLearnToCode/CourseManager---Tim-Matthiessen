using CoursesManager.Application.Dtos.Participants;

namespace CoursesManager.Application.Dtos.Registrations;

public record RegistrationDto(
    int Id,
    DateTime RegisteredAt,
    ParticipantDto Participant,
    int CourseSessionId
);
