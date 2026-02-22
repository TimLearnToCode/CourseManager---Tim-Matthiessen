using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CoursesManager.Presentation.Middlewares
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            //logger
          
            httpContext.Response.StatusCode = exception switch
            {
                _ => StatusCodes.Status500InternalServerError
            };

            return await httpContext.RequestServices
                .GetRequiredService<IProblemDetailsService>()
                .TryWriteAsync(new ProblemDetailsContext
                {
                    HttpContext = httpContext,
                    ProblemDetails = new ProblemDetails
                    {
                        Title = "Server Error",
                        Detail = "An unexpected error occured. Please try again"
                    }
                });
        }
    }
}


//var (statusCode, title, detail) = exception switch
//{
//    UniqueConstraintException => (StatusCodes.Status409Conflict, "Conflict", "Resourse already exists"),
//    _ => (StatusCodes.Status500InternalServerError, "Server Error", "An unexpected error occured")
//};


//var pd = new ProblemDetails { Status = statusCode, Title = title, Detail = detail };

//if (exception is UniqueConstraintException uex)
//{
//    var prop = (uex.ConstraintProperties != null && uex.ConstraintProperties.Count > 0) ? uex.ConstraintProperties[0] : null;
//    var field = prop is null ? null : char.ToLowerInvariant(prop[0]) + prop[1..];

//    pd.Detail = prop is null ? "Resourse already exists." : $"{prop} already exists.";
//    pd.Extensions["code"] = "uniqe_violation";
//    if (field is not null) pd.Extensions["field"] = field;
//}