using CoursesManager.Application.Abstractions.Persistence;
using CoursesManager.Application.Dtos.Courses;
using CoursesManager.Application.Dtos.Participants;
using CoursesManager.Application.Dtos.Registrations;
using CoursesManager.Application.Dtos.Teachers;
using CoursesManager.Application.Services;
using CoursesManager.Infrastructure.Persistence;
using CoursesManager.Infrastructure.Persistence.Repositories;
using CoursesManager.Presentation.Extensions;
using CoursesManager.Presentation.Middlewares;
using EntityFramework.Exceptions.SqlServer;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("LocalDbFile"))
           .UseExceptionProcessor());

// Services
builder.Services.AddScoped<CourseService>();
builder.Services.AddScoped<CourseSessionService>();
builder.Services.AddScoped<TeacherService>();
builder.Services.AddScoped<ParticipantService>();
builder.Services.AddScoped<RegistrationService>();

// Repos
builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<ICourseSessionRepository, CourseSessionRepository>();
builder.Services.AddScoped<ITeacherRepository, TeacherRepository>();
builder.Services.AddScoped<IParticipantRepository, ParticipantRepository>();
builder.Services.AddScoped<IRegistrationRepository, RegistrationRepository>();

builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddValidation();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod());
});

builder.Services.AddProblemDetails(options =>
{
    options.CustomizeProblemDetails = context =>
    {
        context.ProblemDetails.Extensions["requestId"] = context.HttpContext.TraceIdentifier;
        context.ProblemDetails.Extensions["support"] = "support@domain.com";
    };
});
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowFrontend");
app.UseExceptionHandler();


#region Courses

var courses = app.MapGroup("/api/courses").WithTags("Courses");

courses.MapGet("/", async (CourseService courseService, CancellationToken ct) =>
    Results.Ok(await courseService.GetAllCoursesAsync(ct)));

courses.MapGet("/{courseCode}", async (string courseCode, CourseService courseService, CancellationToken ct) =>
{
    var result = await courseService.GetOneCourseAsync(courseCode, ct);
    return result.Match(
        course => Results.Ok(course),
        errors => errors.ToProblemDetails()
    );
});

courses.MapPost("/", async (CreateCourseDto dto, CourseService courseService, CancellationToken ct) =>
{
    var result = await courseService.CreateCourseAsync(dto, ct);
    return result.Match(
        course => Results.Created($"/api/courses/{course.CourseCode}", course),
        errors => errors.ToProblemDetails()
    );
});

courses.MapPut("/{courseCode}", async (string courseCode, UpdateCourseDto dto, CourseService courseService, CancellationToken ct) =>
{
    var result = await courseService.UpdateCourseAsync(courseCode, dto, ct);
    return result.Match(
        course => Results.Ok(course),
        errors => errors.ToProblemDetails()
    );
});

courses.MapDelete("/{courseCode}", async (string courseCode, CourseService courseService, CancellationToken ct) =>
{
    var result = await courseService.DeleteCourseAsync(courseCode, ct);
    return result.Match(
        _ => Results.NoContent(),
        errors => errors.ToProblemDetails()
    );
});

#endregion


#region CourseSessions

var courseSessions = app.MapGroup("/api/course-sessions").WithTags("Course Sessions");

courseSessions.MapGet("/{id:int}", async (int id, CourseSessionService service, CancellationToken ct) =>
{
    var result = await service.GetOneCourseSessionAsync(id, ct);
    return result.Match(
        cs => Results.Ok(cs),
        errors => errors.ToProblemDetails()
    );
});

#endregion


// Grupperar endpoints så vi slipper skriva samma bas-URL flera gånger.
#region Teachers
var teachers = app.MapGroup("/api/teachers").WithTags("Teachers");

teachers.MapGet("/", async (TeacherService service, CancellationToken ct) =>
    Results.Ok(await service.GetAllTeachersAsync(ct)));

teachers.MapGet("/{id:int}", async (int id, TeacherService service, CancellationToken ct) =>
{
    var result = await service.GetOneTeacherAsync(id, ct);
    return result.Match(
        t => Results.Ok(t),
        errors => errors.ToProblemDetails()
    );
});

teachers.MapPost("/", async (CreateTeacherDto dto, TeacherService service, CancellationToken ct) =>
{
    var result = await service.CreateTeacherAsync(dto, ct);
    return result.Match(
        t => Results.Created($"/api/teachers/{t.Id}", t),
        errors => errors.ToProblemDetails()
    );
});

teachers.MapPut("/{id:int}", async (int id, UpdateTeacherDto dto, TeacherService service, CancellationToken ct) =>
{
    var result = await service.UpdateTeacherAsync(id, dto, ct);
    return result.Match(
        t => Results.Ok(t),
        errors => errors.ToProblemDetails()
    );
});

teachers.MapDelete("/{id:int}", async (int id, TeacherService service, CancellationToken ct) =>
{
    var result = await service.DeleteTeacherAsync(id, ct);
    return result.Match(
        _ => Results.NoContent(),
        errors => errors.ToProblemDetails()
    );
});

#endregion


#region Participants

var participants = app.MapGroup("/api/participants").WithTags("Participants");

participants.MapGet("/", async (ParticipantService service, CancellationToken ct) =>
    Results.Ok(await service.GetAllParticipantsAsync(ct)));

participants.MapGet("/{id:int}", async (int id, ParticipantService service, CancellationToken ct) =>
{
    var result = await service.GetOneParticipantAsync(id, ct);
    return result.Match(
        p => Results.Ok(p),
        errors => errors.ToProblemDetails()
    );
});

participants.MapPost("/", async (CreateParticipantDto dto, ParticipantService service, CancellationToken ct) =>
{
    var result = await service.CreateParticipantAsync(dto, ct);
    return result.Match(
        p => Results.Created($"/api/participants/{p.Id}", p),
        errors => errors.ToProblemDetails()
    );
});

participants.MapPut("/{id:int}", async (int id, UpdateParticipantDto dto, ParticipantService service, CancellationToken ct) =>
{
    var result = await service.UpdateParticipantAsync(id, dto, ct);
    return result.Match(
        p => Results.Ok(p),
        errors => errors.ToProblemDetails()
    );
});

participants.MapDelete("/{id:int}", async (int id, ParticipantService service, CancellationToken ct) =>
{
    var result = await service.DeleteParticipantAsync(id, ct);
    return result.Match(
        _ => Results.NoContent(),
        errors => errors.ToProblemDetails()
    );
});

#endregion


#region Registrations

var registrations = app.MapGroup("/api/registrations").WithTags("Registrations");

registrations.MapGet("/session/{courseSessionId:int}", async (int courseSessionId, RegistrationService service, CancellationToken ct) =>
    Results.Ok(await service.GetRegistrationsForSessionAsync(courseSessionId, ct)));

registrations.MapPost("/", async (CreateRegistrationDto dto, RegistrationService service, CancellationToken ct) =>
{
    var result = await service.CreateRegistrationAsync(dto, ct);
    return result.Match(
        r => Results.Created($"/api/registrations/{r.Id}", r),
        errors => errors.ToProblemDetails()
    );
});

registrations.MapDelete("/{id:int}", async (int id, RegistrationService service, CancellationToken ct) =>
{
    var result = await service.DeleteRegistrationAsync(id, ct);
    return result.Match(
        _ => Results.NoContent(),
        errors => errors.ToProblemDetails()
    );
});

#endregion


app.Run();
