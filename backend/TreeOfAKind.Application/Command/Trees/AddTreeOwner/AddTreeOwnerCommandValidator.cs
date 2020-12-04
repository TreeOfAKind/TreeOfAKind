using FluentValidation;

namespace TreeOfAKind.Application.Command.Trees.AddTreeOwner
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
