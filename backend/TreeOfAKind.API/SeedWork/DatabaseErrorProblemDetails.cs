using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TreeOfAKind.Application.Configuration.Authorization;

namespace TreeOfAKind.API.SeedWork
{
    public class DatabaseErrorProblemDetails : ProblemDetails
    {
        public DatabaseErrorProblemDetails(DbUpdateException exception)
        {
            this.Title = "Execution problem";
            this.Status = StatusCodes.Status409Conflict;
            this.Detail = "Please try again";
            this.Type =  "https://httpstatuses.com/" + this.Status;
        }
    }
}
