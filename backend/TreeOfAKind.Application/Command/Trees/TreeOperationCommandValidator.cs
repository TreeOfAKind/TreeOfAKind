using FluentValidation;
using TreeOfAKind.Application.Command.Trees.AddTreeOwner;
using TreeOfAKind.Application.Configuration;
using TreeOfAKind.Domain.Trees;
using TreeOfAKind.Domain.Trees.People;

namespace TreeOfAKind.Application.Command.Trees
{
    public class TreeOperationCommandValidator : AbstractValidator<TreeOperationCommandBase>
    {
        private readonly ITreeRepository _treeRepository;
        public TreeOperationCommandValidator(ITreeRepository treeRepository)
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

    public class TreeOperationCommandValidator<TResponse> : AbstractValidator<TreeOperationCommandBase<TResponse>>
    {
        private readonly ITreeRepository _treeRepository;
        public TreeOperationCommandValidator(ITreeRepository treeRepository)
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
