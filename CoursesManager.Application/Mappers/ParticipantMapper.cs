using CoursesManager.Application.Dtos.Participants;
using CoursesManager.Domain.Entities;
using System.Linq.Expressions;

namespace CoursesManager.Application.Mappers;

public static class ParticipantMapper
{
    public static ParticipantDto ToParticipantDto(ParticipantEntity entity) =>
        new(entity.Id, entity.FirstName, entity.LastName, entity.Email, entity.CreatedAt, entity.UpdatedAt);

    public static Expression<Func<ParticipantEntity, ParticipantDto>> ToParticipantDtoExpr =>
        p => new ParticipantDto(p.Id, p.FirstName, p.LastName, p.Email, p.CreatedAt, p.UpdatedAt);
}
