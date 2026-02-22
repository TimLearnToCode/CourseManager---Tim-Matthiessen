using CoursesManager.Application.Abstractions.Persistence;
using CoursesManager.Application.Common.Errors;
using CoursesManager.Application.Common.Results;
using CoursesManager.Application.Dtos.Registrations;
using CoursesManager.Application.Mappers;
using CoursesManager.Domain.Entities;

namespace CoursesManager.Application.Services;

public class RegistrationService(
    IRegistrationRepository registrationRepository,
    IParticipantRepository participantRepository,
    ICourseSessionRepository courseSessionRepository)
{
    public async Task<IReadOnlyList<RegistrationDto>> GetRegistrationsForSessionAsync(int courseSessionId, CancellationToken ct = default)
    {
        return await registrationRepository.GetAllAsync(
            select: RegistrationMapper.ToRegistrationDtoExpr,
            where: r => r.CourseSessionId == courseSessionId,
            orderBy: o => o.OrderBy(r => r.RegisteredAt),
            ct: ct
        );
    }

    public async Task<ErrorOr<RegistrationDto>> CreateRegistrationAsync(CreateRegistrationDto dto, CancellationToken ct = default)
    {
        var participantExists = await participantRepository.ExistsAsync(p => p.Id == dto.ParticipantId);
        if (!participantExists)
            return Error.NotFound("Participant.NotFound", $"Participant with id '{dto.ParticipantId}' was not found.");

        var sessionExists = await courseSessionRepository.ExistsAsync(cs => cs.Id == dto.CourseSessionId);
        if (!sessionExists)
            return Error.NotFound("CourseSession.NotFound", $"Course session with id '{dto.CourseSessionId}' was not found.");

        // Kollar om deltagaren redan är registrerad innan vi försöker spara.
        var alreadyRegistered = await registrationRepository.ExistsAsync(
            r => r.ParticipantId == dto.ParticipantId && r.CourseSessionId == dto.CourseSessionId);
        if (alreadyRegistered)
            return Error.Conflict("Registration.Conflict", "Participant is already registered for this course session.");

        var saved = await registrationRepository.CreateAsync(new RegistrationEntity
        {
            ParticipantId = dto.ParticipantId,
            CourseSessionId = dto.CourseSessionId,
            RegisteredAt = DateTime.UtcNow
        }, ct);

        // Ladda relationen för att kunna mappa
        var registration = await registrationRepository.GetOneAsync(
            where: r => r.Id == saved.Id,
            tracking: false,
            ct: ct,
            includes: r => r.Participant
        );

        return RegistrationMapper.ToRegistrationDto(registration!);
    }

    public async Task<ErrorOr<Deleted>> DeleteRegistrationAsync(int id, CancellationToken ct = default)
    {
        var registration = await registrationRepository.GetOneAsync(r => r.Id == id, ct);
        if (registration is null)
            return Error.NotFound("Registration.NotFound", $"Registration with id '{id}' was not found.");

        await registrationRepository.DeleteAsync(registration, ct);
        return Result.Deleted;
    }
}
