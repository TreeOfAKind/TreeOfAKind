using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;
using TreeOfAKind.Application.Configuration.Validation;

namespace TreeOfAKind.API
{
    public class ValidateModelStateAttribute : ActionFilterAttribute
    {
        private readonly ILogger _logger;
        public ValidateModelStateAttribute(ILogger logger)
        {
            _logger = logger;
        }
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            StringBuilder sb = new StringBuilder();
            if (!context.ModelState.IsValid)
            {
                sb.AppendLine("Errors:");
                foreach (var error in context.ModelState.Values.SelectMany(v => v.Errors))
                {
                    sb.Append(error.ErrorMessage);
                }
                var errors = sb.ToString();
                _logger.Error("Invalid request: {@Errors}", errors);
                throw new InvalidCommandException("Invalid command", errors);
            }
            base.OnActionExecuted(context);
        }
    }
}
