using CoursesManager.Application.Abstractions.Persistence;
using CoursesManager.Domain.Entities;

namespace CoursesManager.Infrastructure.Persistence.Repositories;

public class CourseRepository(ApplicationDbContext context) : BaseRepository<CourseEntity>(context), ICourseRepository
{
   
}
