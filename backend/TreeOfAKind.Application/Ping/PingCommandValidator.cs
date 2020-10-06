using FluentValidation;

namespace TreeOfAKind.Application.Ping
{
    public class PingCommandValidator : AbstractValidator<PingCommand>
    {
        public PingCommandValidator()
        {
            RuleFor(x => x.PingName).MaximumLength(3);
        }
    }
}