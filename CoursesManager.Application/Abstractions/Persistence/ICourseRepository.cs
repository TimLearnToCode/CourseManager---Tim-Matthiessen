using CoursesManager.Domain.Entities;

namespace CoursesManager.Application.Abstractions.Persistence;

public interface ICourseRepository : IBaseRepository<CourseEntity>
{
}
