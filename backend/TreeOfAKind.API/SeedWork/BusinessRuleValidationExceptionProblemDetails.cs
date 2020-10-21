using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using TreeOfAKind.Domain.SeedWork;
using TreeOfAKind.Domain.UserProfiles;

namespace TreeOfAKind.API.SeedWork
{
    public class BusinessRuleValidationExceptionProblemDetails : Microsoft.AspNetCore.Mvc.ProblemDetails
    {
        private static readonly Dictionary<Type, int> BusinessRuleToStatus = new Dictionary<Type, int>()
        {
            {typeof(AuthUserIdMustBeUniqueRule), StatusCodes.Status422UnprocessableEntity}
        };

        public BusinessRuleValidationExceptionProblemDetails(BusinessRuleValidationException exception)
        {
            var brokenRuleType = exception.BrokenRule.GetType();
            this.Title = "Business rule validation error";
            this.Status = BusinessRuleToStatus?[brokenRuleType] ?? StatusCodes.Status409Conflict;
            this.Detail = exception.Details;
            this.Type = "https://httpstatuses.com/" + this.Status;
        }
    }
}