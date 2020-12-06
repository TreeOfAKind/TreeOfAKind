using FluentValidation;
using TreeOfAKind.Application.Configuration;

namespace TreeOfAKind.Application.Command.Trees.AddPerson
{
    public class AddPersonCommandValidator : AbstractValidator<AddPersonCommand>
    {
        public class RelationValidator : AbstractValidator<AddPersonCommand.Relation>
        {
            public RelationValidator()
            {
                RuleFor(x => x.RelationType)
                    .NotNull();

                RuleFor(x => x.SecondPersonId)
                    .NotEmpty();
            }
        }
        public AddPersonCommandValidator()
        {
            RuleFor(x => x.Name)
                .MaximumLength(StringLengths.VeryShort);

            RuleFor(x => x.Surname)
                .MaximumLength(StringLengths.VeryShort);

            RuleFor(x => x.Description)
                .MaximumLength(StringLengths.Short);

            RuleFor(x => x.Biography)
                .MaximumLength(StringLengths.VeryLong);

            RuleForEach(x => x.Relations)
                .SetValidator(new RelationValidator());
        }
    }
}
