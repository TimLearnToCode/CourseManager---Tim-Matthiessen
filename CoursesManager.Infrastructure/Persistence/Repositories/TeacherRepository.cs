using CoursesManager.Application.Abstractions.Persistence;
using CoursesManager.Domain.Entities;

namespace CoursesManager.Infrastructure.Persistence.Repositories;

public class TeacherRepository(ApplicationDbContext context) : BaseRepository<TeacherEntity>(context), ITeacherRepository
{
}
