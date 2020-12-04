using FluentValidation;
using TreeOfAKind.Application.Configuration;

namespace TreeOfAKind.Application.Command.Trees.AddPerson
{
    public class AddPersonCommandValidator : AbstractValidator<AddPersonCommand>
    {
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
        }
    }
}