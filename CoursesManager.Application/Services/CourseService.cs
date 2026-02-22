using CoursesManager.Application.Abstractions.Persistence;
using CoursesManager.Application.Common.Errors;
using CoursesManager.Application.Common.Results;
using CoursesManager.Application.Dtos.Courses;
using CoursesManager.Application.Mappers;
using CoursesManager.Domain.Entities;

namespace CoursesManager.Application.Services;

public class CourseService(ICourseRepository courseRepositry)
{
    private readonly ICourseRepository _courseRepositry = courseRepositry;

    public async Task<ErrorOr<CourseDto>> CreateCourseAsync(CreateCourseDto dto, CancellationToken ct = default)
    {
        var exists = await _courseRepositry.ExistsAsync(x => x.CourseCode == dto.CourseCode);
        if (exists)
            return Error.Conflict("Course.Conflict", $"Course with '{dto.CourseCode}' already exists.");

        var savedCourse = await _courseRepositry.CreateAsync(new CourseEntity { CourseCode = dto.CourseCode, Title = dto.Title, Description = dto.Description }, ct);
        return CourseMapper.ToCourseDto(savedCourse);
    }

    public async Task<ErrorOr<CourseDto>> GetOneCourseAsync(string courseCode, CancellationToken ct = default)
    {
        var course = await _courseRepositry.GetOneAsync(x => x.CourseCode == courseCode, ct);
        return course is not null 
            ? CourseMapper.ToCourseDto(course) 
            : Error.NotFound("Courses.NotFound", $"Course with '{courseCode}' was not found.");
    }

    public async Task<IReadOnlyList<CourseDto>> GetAllCoursesAsync(CancellationToken ct = default)
    {
        return await _courseRepositry.GetAllAsync(
            select: c => new CourseDto(c.CourseCode, c.Title, c.Description,  c.CreatedAt, c.UpdatedAt, c.RowVersion ),
            orderBy: o => o.OrderByDescending(x => x.CreatedAt),
            ct: ct
        );
    }

    public async Task<ErrorOr<CourseDto>> UpdateCourseAsync(string courseCode, UpdateCourseDto dto, CancellationToken ct = default)
    {
        var course = await _courseRepositry.GetOneAsync(x => x.CourseCode ==  courseCode, ct);
        if (course is null)
            return Error.NotFound("Courses.NotFound", $"Course with '{courseCode}' was not found.");


        // Kollar att ingen annan hunnit ändra kursen innan vi sparar.
        // Om så är fallet får användaren ett felmeddelande istället.
        if (!course.RowVersion.SequenceEqual(dto.RowVersion))
            return Error.Conflict("Courses.Conflict", "Updated by another user. Try again.");

        course.Title = dto.Title;
        course.Description = dto.Description;
        course.UpdatedAt = DateTime.UtcNow;

        await _courseRepositry.SaveChangesAsync(ct);
        return CourseMapper.ToCourseDto(course);
    }

    public async Task<ErrorOr<Deleted>> DeleteCourseAsync(string courseCode, CancellationToken ct = default)
    {
        var course = await _courseRepositry.GetOneAsync(x => x.CourseCode == courseCode, ct);
        if (course is null)
            return Error.NotFound("Course.NotFound", $"Course with {courseCode} was not found.");

        await _courseRepositry.DeleteAsync(course, ct);
        return Result.Deleted;
    }
}