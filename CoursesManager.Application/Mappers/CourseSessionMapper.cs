using CoursesManager.Application.Dtos.Courses;
using CoursesManager.Application.Dtos.CourseSessions;
using CoursesManager.Application.Dtos.Locations;
using CoursesManager.Domain.Entities;
using System.Linq.Expressions;

namespace CoursesManager.Application.Mappers;

public class CourseSessionMapper
{

    // Expression gör att EF Core kan översätta mappningen till SQL direkt
    // istället för att hämta hela raden och mappa den i C#.
    public static Expression<Func<CourseSessionEntity, CourseSessionDto>> ToCourseSessionDtoExpr =>
        cs => new CourseSessionDto(
            cs.Id,
            cs.StartDate,
            cs.EndDate,
            cs.MaxParticipants,
            cs.CreatedAt,
            cs.UpdatedAt,
            new CourseDto(
                cs.Course.CourseCode,
                cs.Course.Title,
                cs.Course.Description,
                cs.Course.CreatedAt,
                cs.Course.UpdatedAt,
                cs.Course.RowVersion
            ),
            new LocationDto(
                cs.Location.Name
            )
        );


    public static CourseSessionDto ToCourseSessionDto(CourseSessionEntity entity) => new
    (
        entity.Id,
        entity.StartDate,
        entity.EndDate,
        entity.MaxParticipants,
        entity.CreatedAt,
        entity.UpdatedAt,
        new CourseDto
        (
            entity.Course.CourseCode,
            entity.Course.Title,
            entity.Course.Description,
            entity.Course.CreatedAt,
            entity.Course.UpdatedAt,
            entity.Course.RowVersion
        ),
        new LocationDto(entity.Location.Name)
    );
  
}
