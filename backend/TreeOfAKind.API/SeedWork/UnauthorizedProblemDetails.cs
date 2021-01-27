using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TreeOfAKind.Application.Configuration.Authorization;

namespace TreeOfAKind.API.SeedWork
{
    public class UnauthorizedProblemDetails : ProblemDetails
    {
        public UnauthorizedProblemDetails(UnauthorizedException exception)
        {
            this.Title = exception.Message;
            this.Status = StatusCodes.Status403Forbidden;
            this.Detail = exception.Details;
            this.Type =  "https://httpstatuses.com/" + this.Status;
        }
    }
}
