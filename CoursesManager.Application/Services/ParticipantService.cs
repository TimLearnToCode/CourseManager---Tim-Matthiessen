using CoursesManager.Application.Abstractions.Persistence;
using CoursesManager.Application.Common.Errors;
using CoursesManager.Application.Common.Results;
using CoursesManager.Application.Dtos.Participants;
using CoursesManager.Application.Mappers;
using CoursesManager.Domain.Entities;

namespace CoursesManager.Application.Services;

public class ParticipantService(IParticipantRepository participantRepository)
{
    public async Task<IReadOnlyList<ParticipantDto>> GetAllParticipantsAsync(CancellationToken ct = default)
    {
        return await participantRepository.GetAllAsync(
            select: ParticipantMapper.ToParticipantDtoExpr,
            orderBy: o => o.OrderBy(p => p.LastName),
            ct: ct
        );
    }

    public async Task<ErrorOr<ParticipantDto>> GetOneParticipantAsync(int id, CancellationToken ct = default)
    {
        var participant = await participantRepository.GetOneAsync(p => p.Id == id, ct);
        return participant is not null
            ? ParticipantMapper.ToParticipantDto(participant)
            : Error.NotFound("Participant.NotFound", $"Participant with id '{id}' was not found.");
    }

    public async Task<ErrorOr<ParticipantDto>> CreateParticipantAsync(CreateParticipantDto dto, CancellationToken ct = default)
    {
        var exists = await participantRepository.ExistsAsync(p => p.Email == dto.Email);
        if (exists)
            return Error.Conflict("Participant.Conflict", $"A participant with email '{dto.Email}' already exists.");

        var saved = await participantRepository.CreateAsync(new ParticipantEntity
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email
        }, ct);

        return ParticipantMapper.ToParticipantDto(saved);
    }

    public async Task<ErrorOr<ParticipantDto>> UpdateParticipantAsync(int id, UpdateParticipantDto dto, CancellationToken ct = default)
    {
        var participant = await participantRepository.GetOneAsync(p => p.Id == id, ct);
        if (participant is null)
            return Error.NotFound("Participant.NotFound", $"Participant with id '{id}' was not found.");

        var emailTaken = await participantRepository.ExistsAsync(p => p.Email == dto.Email && p.Id != id);
        if (emailTaken)
            return Error.Conflict("Participant.Conflict", $"Email '{dto.Email}' is already in use.");

        participant.FirstName = dto.FirstName;
        participant.LastName = dto.LastName;
        participant.Email = dto.Email;
        participant.UpdatedAt = DateTime.UtcNow;

        await participantRepository.SaveChangesAsync(ct);
        return ParticipantMapper.ToParticipantDto(participant);
    }

    public async Task<ErrorOr<Deleted>> DeleteParticipantAsync(int id, CancellationToken ct = default)
    {
        var participant = await participantRepository.GetOneAsync(p => p.Id == id, ct);
        if (participant is null)
            return Error.NotFound("Participant.NotFound", $"Participant with id '{id}' was not found.");

        await participantRepository.DeleteAsync(participant, ct);
        return Result.Deleted;
    }
}
