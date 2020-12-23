using FluentValidation;

namespace TreeOfAKind.Application.Command.Trees.TreeAdministration.AddTreeOwner
{
    public class AddTreeOwnerCommandValidator : AbstractValidator<AddTreeOwnerCommand>
    {
        public AddTreeOwnerCommandValidator()
        {
            RuleFor(x => x.AddedPersonMailAddress)
                .EmailAddress()
                .WithMessage("Provided string is not proper e-mail address");
        }
    }
}
