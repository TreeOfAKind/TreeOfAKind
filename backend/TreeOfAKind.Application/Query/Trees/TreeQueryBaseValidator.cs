using FluentValidation;
using TreeOfAKind.Application.Configuration;
using TreeOfAKind.Domain.Trees;

namespace TreeOfAKind.Application.Query.Trees
{
    public class TreeQueryBaseValidator<TResponse> : AbstractValidator<TreeQueryBase<TResponse>>
    {
        public TreeQueryBaseValidator(ITreeRepository treeRepository)
        {
            RuleFor(x => x.TreeId)
                .NotEmpty();

            RuleFor(x => x.RequesterUserAuthId)
                .NotEmpty()
                .MaximumLength(StringLengths.AuthIdLength);
        }
    }
}
