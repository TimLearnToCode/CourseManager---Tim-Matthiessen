using CoursesManager.Application.Abstractions.Persistence;
using CoursesManager.Application.Dtos.Teachers;
using CoursesManager.Application.Services;
using CoursesManager.Domain.Entities;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Xunit;

namespace CoursesManager.Tests;

public class TeacherServiceTests
{
    private readonly ITeacherRepository _teacherRepository;
    private readonly TeacherService _sut;

    public TeacherServiceTests()
    {
        _teacherRepository = Substitute.For<ITeacherRepository>();
        _sut = new TeacherService(_teacherRepository);
    }

    [Fact]
    public async Task CreateTeacherAsync_ShouldReturnConflict_WhenEmailAlreadyExists()
    {
        // Arrange
        var dto = new CreateTeacherDto("Anna", "Svensson", "anna@test.se");
        _teacherRepository.ExistsAsync(Arg.Any<System.Linq.Expressions.Expression<Func<TeacherEntity, bool>>>())
            .Returns(true);

        // Act
        var result = await _sut.CreateTeacherAsync(dto);

        // Assert
        Assert.True(result.IsError);
        Assert.Equal("Teacher.Conflict", result.FirstError.Code);
    }

    [Fact]
    public async Task CreateTeacherAsync_ShouldReturnTeacherDto_WhenCreatedSuccessfully()
    {
        // Arrange
        var dto = new CreateTeacherDto("Anna", "Svensson", "anna@test.se");
        var entity = new TeacherEntity
        {
            Id = 1,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _teacherRepository.ExistsAsync(Arg.Any<System.Linq.Expressions.Expression<Func<TeacherEntity, bool>>>())
            .Returns(false);
        _teacherRepository.CreateAsync(Arg.Any<TeacherEntity>(), Arg.Any<CancellationToken>())
            .Returns(entity);

        // Act
        var result = await _sut.CreateTeacherAsync(dto);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("anna@test.se", result.Value.Email);
    }

    [Fact]
    public async Task GetOneTeacherAsync_ShouldReturnNotFound_WhenTeacherDoesNotExist()
    {
        // Arrange
        _teacherRepository.GetOneAsync(Arg.Any<System.Linq.Expressions.Expression<Func<TeacherEntity, bool>>>(), Arg.Any<CancellationToken>())
            .ReturnsNull();

        // Act
        var result = await _sut.GetOneTeacherAsync(999);

        // Assert
        Assert.True(result.IsError);
        Assert.Equal("Teacher.NotFound", result.FirstError.Code);
    }
}
