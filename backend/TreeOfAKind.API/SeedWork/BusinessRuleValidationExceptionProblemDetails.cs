using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TreeOfAKind.Domain.SeedWork;

namespace TreeOfAKind.API.SeedWork
{
    public class BusinessRuleValidationExceptionProblemDetails : ProblemDetails
    {
        public BusinessRuleValidationExceptionProblemDetails(BusinessRuleValidationException exception)
        {
            var brokenRuleType = exception.BrokenRule.GetType();
            this.Title = "Business rule validation error";
            this.Status = StatusCodes.Status422UnprocessableEntity;
            this.Detail = exception.Details;
            this.Type = "https://httpstatuses.com/" + this.Status;
            this.Extensions.Add("errorCode", exception.BrokenRule.GetType().ToString());
        }
    }
}