using FluentValidation;

namespace TreeOfAKind.Application.Command.Trees.People
{
    public class RelationValidator : AbstractValidator<Relation>
    {
        public RelationValidator()
        {
            RuleFor(x => x.RelationType)
                .NotNull();

            RuleFor(x => x.SecondPersonId)
                .NotEmpty();
        }
    }
}
