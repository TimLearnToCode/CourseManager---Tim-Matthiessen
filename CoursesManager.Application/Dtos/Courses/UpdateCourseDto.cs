using System.ComponentModel.DataAnnotations;

namespace CoursesManager.Application.Dtos.Courses
{
    public record UpdateCourseDto(
        [Required, MinLength(1), MaxLength(50)] string Title,
        [Required, MinLength(1), MaxLength(200)] string Description,
        [Required] byte[] RowVersion
    );
}

