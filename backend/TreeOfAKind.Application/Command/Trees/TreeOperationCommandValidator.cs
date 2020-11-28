using FluentValidation;
using TreeOfAKind.Application.Command.Trees.AddTreeOwner;
using TreeOfAKind.Application.Configuration;

namespace TreeOfAKind.Application.Command.Trees
{
    public class TreeOperationCommandValidator : AbstractValidator<TreeOperationCommand>
    {
        public TreeOperationCommandValidator()
        {
            RuleFor(x => x.TreeId)
                .NotEmpty();

            RuleFor(x => x.RequesterAuthUserId)
                .NotEmpty()
                .MaximumLength(StringLengths.AuthIdLength);
        }
    }
}