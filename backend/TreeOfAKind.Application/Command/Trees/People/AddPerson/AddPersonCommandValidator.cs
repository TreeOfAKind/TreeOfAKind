using FluentValidation;
using TreeOfAKind.Application.Configuration;

namespace TreeOfAKind.Application.Command.Trees.People.AddPerson
{
    public class AddPersonCommandValidator : AbstractValidator<AddPersonCommand>
    {

        public AddPersonCommandValidator()
        {
            RuleFor(x => x.Name)
                .MaximumLength(StringLengths.VeryShort);

            RuleFor(x => x.LastName)
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
