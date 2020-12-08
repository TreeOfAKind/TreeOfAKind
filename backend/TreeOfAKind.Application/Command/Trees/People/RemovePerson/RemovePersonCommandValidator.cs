using FluentValidation;

namespace TreeOfAKind.Application.Command.Trees.People.RemovePerson
{
    public class RemovePersonCommandValidator : AbstractValidator<RemovePersonCommand>
    {
        public RemovePersonCommandValidator()
        {
            RuleFor(x => x.PersonId)
                .NotEmpty();
        }
    }
}
