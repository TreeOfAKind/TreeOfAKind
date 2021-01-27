using FluentValidation;

namespace TreeOfAKind.Application.Command.Trees.People.AddRelation
{
    public class AddRelationValidator : AbstractValidator<AddRelationCommand>
    {
        public AddRelationValidator()
        {
            RuleFor(x => x.From)
                .NotEmpty();

            RuleFor(x => x.To)
                .NotEmpty();
        }
    }
}