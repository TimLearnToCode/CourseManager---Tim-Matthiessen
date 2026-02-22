using CoursesManager.Application.Dtos.Participants;
using CoursesManager.Application.Dtos.Registrations;
using CoursesManager.Domain.Entities;
using System.Linq.Expressions;

namespace CoursesManager.Application.Mappers;

public static class RegistrationMapper
{
    public static RegistrationDto ToRegistrationDto(RegistrationEntity entity) =>
        new(
            entity.Id,
            entity.RegisteredAt,
            new ParticipantDto(entity.Participant.Id, entity.Participant.FirstName, entity.Participant.LastName, entity.Participant.Email, entity.Participant.CreatedAt, entity.Participant.UpdatedAt),
            entity.CourseSessionId
        );

    public static Expression<Func<RegistrationEntity, RegistrationDto>> ToRegistrationDtoExpr =>
        r => new RegistrationDto(
            r.Id,
            r.RegisteredAt,
            new ParticipantDto(r.Participant.Id, r.Participant.FirstName, r.Participant.LastName, r.Participant.Email, r.Participant.CreatedAt, r.Participant.UpdatedAt),
            r.CourseSessionId
        );
}
