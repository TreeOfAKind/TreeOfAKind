using FluentValidation;

namespace TreeOfAKind.Application.Command.Trees.TreeAdministration.MergeTrees
{
    public class MergeTreesCommandValidator : AbstractValidator<MergeTreesCommand>
    {
        public MergeTreesCommandValidator()
        {
            RuleFor(x => x.First)
                .NotEmpty();

            RuleFor(x => x.Second)
                .NotEmpty();

            RuleFor(x => x.RequesterUserAuthId)
                .NotEmpty();

            RuleFor(x => x.First)
                .NotEqual(x => x.Second)
                .WithMessage("Cannot merge tree to itself");
        }
    }
}
