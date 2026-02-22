using CoursesManager.Application.Abstractions.Persistence;
using CoursesManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CoursesManager.Infrastructure.Persistence.Repositories;

public class CourseSessionRepository(ApplicationDbContext context) : BaseRepository<CourseSessionEntity>(context), ICourseSessionRepository
{
    public override async Task<CourseSessionEntity?> GetOneAsync(Expression<Func<CourseSessionEntity, bool>> where, CancellationToken ct = default)
    {
        return await _context.CourseSessions.Include(x => x.Course).Include(x => x.Location).FirstOrDefaultAsync(where, ct);
    }
}