using FluentValidation;
using TreeOfAKind.Application.Configuration;

namespace TreeOfAKind.Application.Command.Trees
{
    public class TreeOperationCommandValidator : AbstractValidator<TreeOperationCommandBase>
    {
        public TreeOperationCommandValidator()
        {
            RuleFor(x => x.TreeId)
                .NotEmpty()
                .WithMessage($"{nameof(TreeOperationCommandBase.TreeId)} is not valid.");

            RuleFor(x => x.RequesterUserAuthId)
                .NotEmpty()
                .MaximumLength(StringLengths.AuthIdLength);
        }
    }

    public class TreeOperationCommandValidator<TResponse> : AbstractValidator<TreeOperationCommandBase<TResponse>>
    {
        public TreeOperationCommandValidator()
        {
            RuleFor(x => x.TreeId)
                .NotEmpty()
                .WithMessage($"{nameof(TreeOperationCommandBase.TreeId)} is not valid.");

            RuleFor(x => x.RequesterUserAuthId)
                .NotEmpty()
                .MaximumLength(StringLengths.AuthIdLength);
        }
    }
}
