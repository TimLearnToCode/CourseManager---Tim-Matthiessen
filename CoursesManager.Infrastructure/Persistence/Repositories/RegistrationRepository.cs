using CoursesManager.Application.Abstractions.Persistence;
using CoursesManager.Domain.Entities;

namespace CoursesManager.Infrastructure.Persistence.Repositories;

public class RegistrationRepository(ApplicationDbContext context) : BaseRepository<RegistrationEntity>(context), IRegistrationRepository
{
}
