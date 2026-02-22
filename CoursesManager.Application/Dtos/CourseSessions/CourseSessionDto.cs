using CoursesManager.Application.Dtos.Courses;
using CoursesManager.Application.Dtos.Locations;

namespace CoursesManager.Application.Dtos.CourseSessions;

public record CourseSessionDto
(
    int Id,
    DateTime StartDate,
    DateTime EndDate,
    int MaxParticipants,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    CourseDto Course,
    LocationDto Location
);

