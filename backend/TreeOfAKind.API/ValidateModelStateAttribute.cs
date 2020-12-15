using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Mvc.Filters;
using TreeOfAKind.Application.Configuration.Validation;

namespace TreeOfAKind.API
{
    public class ValidateModelStateAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (!context.ModelState.IsValid)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Errors:");
                foreach (var error in context.ModelState.Values.SelectMany(v => v.Errors))
                {
                    sb.AppendLine(error.ErrorMessage);
                }
                throw new InvalidCommandException("Invalid command", sb.ToString());
            }
            base.OnActionExecuted(context);
        }
    }
}
