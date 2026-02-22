using CoursesManager.Application.Abstractions.Persistence;
using CoursesManager.Application.Dtos.Courses;
using CoursesManager.Application.Services;
using CoursesManager.Domain.Entities;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Xunit;

namespace CoursesManager.Tests;

public class CourseServiceTests
{
    private readonly ICourseRepository _courseRepository;
    private readonly CourseService _sut;

    public CourseServiceTests()
    {
        _courseRepository = Substitute.For<ICourseRepository>();
        _sut = new CourseService(_courseRepository);
    }

    [Fact]
    public async Task CreateCourseAsync_ShouldReturnConflict_WhenCourseCodeAlreadyExists()
    {
        var dto = new CreateCourseDto { CourseCode = "CS101", Title = "C# Basics", Description = "An intro course" };
        _courseRepository.ExistsAsync(Arg.Any<System.Linq.Expressions.Expression<Func<CourseEntity, bool>>>())
            .Returns(true);

        var result = await _sut.CreateCourseAsync(dto);

        Assert.True(result.IsError);
        Assert.Equal("Course.Conflict", result.FirstError.Code);
    }

    [Fact]
    public async Task CreateCourseAsync_ShouldReturnCourseDto_WhenCourseIsCreatedSuccessfully()
    {
        var dto = new CreateCourseDto { CourseCode = "CS102", Title = "C# Advanced", Description = "An advanced course" };
        var savedEntity = new CourseEntity
        {
            CourseId = 1,
            CourseCode = dto.CourseCode,
            Title = dto.Title,
            Description = dto.Description,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            RowVersion = []
        };

        _courseRepository.ExistsAsync(Arg.Any<System.Linq.Expressions.Expression<Func<CourseEntity, bool>>>())
            .Returns(false);
        _courseRepository.CreateAsync(Arg.Any<CourseEntity>(), Arg.Any<CancellationToken>())
            .Returns(savedEntity);

        var result = await _sut.CreateCourseAsync(dto);

        Assert.True(result.IsSuccess);
        Assert.Equal(dto.CourseCode, result.Value.CourseCode);
    }

    [Fact]
    public async Task GetOneCourseAsync_ShouldReturnNotFound_WhenCourseDoesNotExist()
    {
        _courseRepository.GetOneAsync(Arg.Any<System.Linq.Expressions.Expression<Func<CourseEntity, bool>>>(), Arg.Any<CancellationToken>())
            .ReturnsNull();

        var result = await _sut.GetOneCourseAsync("NONEXISTENT");

        Assert.True(result.IsError);
        Assert.Equal("Courses.NotFound", result.FirstError.Code);
    }

    [Fact]
    public async Task GetOneCourseAsync_ShouldReturnCourseDto_WhenCourseExists()
    {
        var entity = new CourseEntity
        {
            CourseId = 1,
            CourseCode = "CS101",
            Title = "C# Basics",
            Description = "Intro",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            RowVersion = []
        };

        _courseRepository.GetOneAsync(Arg.Any<System.Linq.Expressions.Expression<Func<CourseEntity, bool>>>(), Arg.Any<CancellationToken>())
            .Returns(entity);

        var result = await _sut.GetOneCourseAsync("CS101");

        Assert.True(result.IsSuccess);
        Assert.Equal("CS101", result.Value.CourseCode);
    }

    [Fact]
    public async Task DeleteCourseAsync_ShouldReturnNotFound_WhenCourseDoesNotExist()
    {
        _courseRepository.GetOneAsync(Arg.Any<System.Linq.Expressions.Expression<Func<CourseEntity, bool>>>(), Arg.Any<CancellationToken>())
            .ReturnsNull();

        var result = await _sut.DeleteCourseAsync("NONEXISTENT");

        Assert.True(result.IsError);
        Assert.Equal("Course.NotFound", result.FirstError.Code);
    }
}