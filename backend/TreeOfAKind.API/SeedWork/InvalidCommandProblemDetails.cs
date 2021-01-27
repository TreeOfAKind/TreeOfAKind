using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TreeOfAKind.Application.Configuration.Validation;

namespace TreeOfAKind.API.SeedWork
{
    public class InvalidCommandProblemDetails : ProblemDetails
    {
        public InvalidCommandProblemDetails(InvalidCommandException exception)
        {
            this.Title = exception.Message;
            this.Status = StatusCodes.Status400BadRequest;
            this.Detail = exception.Details;
            this.Type =  "https://httpstatuses.com/" + this.Status;
        }
    }
}