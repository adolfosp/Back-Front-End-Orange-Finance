using System.Net;

using Microsoft.AspNetCore.Mvc;

namespace OrangeFinance.Extensions;

public static class ProblemsDetailsResult
{
    public static IResult GetProblemsDetails(this List<ErrorOr.Error> errors)
    {
        var problems = new List<ProblemDetails>();

        foreach (var error in errors)
        {
            var problemDetails = new ProblemDetails
            {
                Status = (int?)HttpStatusCode.BadRequest,
                Type = error.Type.ToString(),
                Title = error.Description,
            };
            problems.Add(problemDetails);
        }

        return Results.BadRequest(problems);
    }
}
