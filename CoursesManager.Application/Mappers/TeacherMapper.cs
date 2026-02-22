using CoursesManager.Application.Dtos.Teachers;
using CoursesManager.Domain.Entities;
using System.Linq.Expressions;

namespace CoursesManager.Application.Mappers;

public static class TeacherMapper
{
    public static TeacherDto ToTeacherDto(TeacherEntity entity) =>
        new(entity.Id, entity.FirstName, entity.LastName, entity.Email, entity.CreatedAt, entity.UpdatedAt);

    public static Expression<Func<TeacherEntity, TeacherDto>> ToTeacherDtoExpr =>
        t => new TeacherDto(t.Id, t.FirstName, t.LastName, t.Email, t.CreatedAt, t.UpdatedAt);
}
