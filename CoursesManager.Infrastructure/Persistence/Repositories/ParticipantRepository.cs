using CoursesManager.Application.Abstractions.Persistence;
using CoursesManager.Domain.Entities;

namespace CoursesManager.Infrastructure.Persistence.Repositories;

public class ParticipantRepository(ApplicationDbContext context) : BaseRepository<ParticipantEntity>(context), IParticipantRepository
{
}
