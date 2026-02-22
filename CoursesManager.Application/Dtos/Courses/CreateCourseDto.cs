using System.ComponentModel.DataAnnotations;

namespace CoursesManager.Application.Dtos.Courses;

public class CreateCourseDto
{
    [Required]
    [MinLength(1)]
    [MaxLength(20)]
    public string CourseCode { get; set; } = null!;

    [Required]
    [MinLength(1)]
    [MaxLength(50)]
    public string Title { get; set; } = null!;

    [Required]
    [MinLength(1)]
    [MaxLength(200)]
    public string Description { get; set; } = null!;
}
