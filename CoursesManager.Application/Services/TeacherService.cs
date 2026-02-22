using CoursesManager.Application.Abstractions.Persistence;
using CoursesManager.Application.Common.Errors;
using CoursesManager.Application.Common.Results;
using CoursesManager.Application.Dtos.Teachers;
using CoursesManager.Application.Mappers;
using CoursesManager.Domain.Entities;

namespace CoursesManager.Application.Services;

public class TeacherService(ITeacherRepository teacherRepository)
{
    public async Task<IReadOnlyList<TeacherDto>> GetAllTeachersAsync(CancellationToken ct = default)
    {
        return await teacherRepository.GetAllAsync(
            select: TeacherMapper.ToTeacherDtoExpr,
            orderBy: o => o.OrderBy(t => t.LastName),
            ct: ct
        );
    }

    public async Task<ErrorOr<TeacherDto>> GetOneTeacherAsync(int id, CancellationToken ct = default)
    {
        var teacher = await teacherRepository.GetOneAsync(t => t.Id == id, ct);
        return teacher is not null
            ? TeacherMapper.ToTeacherDto(teacher)
            : Error.NotFound("Teacher.NotFound", $"Teacher with id '{id}' was not found.");
    }

    public async Task<ErrorOr<TeacherDto>> CreateTeacherAsync(CreateTeacherDto dto, CancellationToken ct = default)
    {
        var exists = await teacherRepository.ExistsAsync(t => t.Email == dto.Email);
        if (exists)
            return Error.Conflict("Teacher.Conflict", $"A teacher with email '{dto.Email}' already exists.");

        var saved = await teacherRepository.CreateAsync(new TeacherEntity
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email
        }, ct);

        return TeacherMapper.ToTeacherDto(saved);
    }

    public async Task<ErrorOr<TeacherDto>> UpdateTeacherAsync(int id, UpdateTeacherDto dto, CancellationToken ct = default)
    {
        var teacher = await teacherRepository.GetOneAsync(t => t.Id == id, ct);
        if (teacher is null)
            return Error.NotFound("Teacher.NotFound", $"Teacher with id '{id}' was not found.");

        var emailTaken = await teacherRepository.ExistsAsync(t => t.Email == dto.Email && t.Id != id);
        if (emailTaken)
            return Error.Conflict("Teacher.Conflict", $"Email '{dto.Email}' is already in use.");

        teacher.FirstName = dto.FirstName;
        teacher.LastName = dto.LastName;
        teacher.Email = dto.Email;
        teacher.UpdatedAt = DateTime.UtcNow;

        await teacherRepository.SaveChangesAsync(ct);
        return TeacherMapper.ToTeacherDto(teacher);
    }

    public async Task<ErrorOr<Deleted>> DeleteTeacherAsync(int id, CancellationToken ct = default)
    {
        var teacher = await teacherRepository.GetOneAsync(t => t.Id == id, ct);
        if (teacher is null)
            return Error.NotFound("Teacher.NotFound", $"Teacher with id '{id}' was not found.");

        await teacherRepository.DeleteAsync(teacher, ct);
        return Result.Deleted;
    }
}
