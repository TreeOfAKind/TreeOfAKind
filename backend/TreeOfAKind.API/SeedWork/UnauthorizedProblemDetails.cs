using Microsoft.AspNetCore.Http;
using TreeOfAKind.Application.Configuration.Authorization;

namespace TreeOfAKind.API.SeedWork
{
    public class UnauthorizedProblemDetails : Microsoft.AspNetCore.Mvc.ProblemDetails
    {
        public UnauthorizedProblemDetails(UnauthorizedException exception)
        {
            this.Title = exception.Message;
            this.Status = StatusCodes.Status403Forbidden;
            this.Detail = exception.Details;
            this.Type = "https://somedomain/validation-error";
        }
    }
}
