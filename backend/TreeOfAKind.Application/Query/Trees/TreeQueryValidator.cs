using FluentValidation;
using TreeOfAKind.Application.Command.Trees;
using TreeOfAKind.Application.Configuration;
using TreeOfAKind.Domain.Trees;

namespace TreeOfAKind.Application.Query.Trees
{
    public class TreeQueryValidator<TResponse> : AbstractValidator<TreeQueryBase<TResponse>>
    {
        private readonly ITreeRepository _treeRepository;

        public TreeQueryValidator(ITreeRepository treeRepository)
        {
            _treeRepository = treeRepository;

            RuleFor(x => x.TreeId)
                .NotEmpty()
                .MustAsync(async (x, c) => (await _treeRepository.GetByIdAsync(x, c)) is {})
                .WithMessage($"{nameof(TreeOperationCommandBase.TreeId)} is not valid.");

            RuleFor(x => x.RequesterUserAuthId)
                .NotEmpty()
                .MaximumLength(StringLengths.AuthIdLength);
        }
    }
}
