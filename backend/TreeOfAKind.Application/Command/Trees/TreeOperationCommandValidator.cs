using FluentValidation;
using TreeOfAKind.Application.Configuration;

namespace TreeOfAKind.Application.Command.Trees
{
    public class TreeOperationCommandValidator : AbstractValidator<TreeOperationCommand>
    {
        public TreeOperationCommandValidator()
        {
            RuleFor(x => x.TreeId)
                .NotEmpty()
                .WithMessage($"{nameof(TreeOperationCommand.TreeId)} is not valid.");

            RuleFor(x => x.RequesterUserAuthId)
                .NotEmpty()
                .MaximumLength(StringLengths.AuthIdLength);
        }
    }
}
