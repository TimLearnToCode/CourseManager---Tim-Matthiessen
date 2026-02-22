using CoursesManager.Application.Dtos.Courses;
using CoursesManager.Domain.Entities;

namespace CoursesManager.Application.Mappers;
public class CourseMapper
{
    public static CourseDto ToCourseDto(CourseEntity entity) => new
    (
        entity.CourseCode,
        entity.Title,
        entity.Description,
        entity.CreatedAt,
        entity.UpdatedAt,
        entity.RowVersion
    );
}

