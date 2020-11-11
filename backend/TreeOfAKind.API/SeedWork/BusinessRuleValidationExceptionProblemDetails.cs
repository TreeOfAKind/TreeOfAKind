using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using TreeOfAKind.Domain.SeedWork;
using TreeOfAKind.Domain.UserProfiles;

namespace TreeOfAKind.API.SeedWork
{
    public class BusinessRuleValidationExceptionProblemDetails : Microsoft.AspNetCore.Mvc.ProblemDetails
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