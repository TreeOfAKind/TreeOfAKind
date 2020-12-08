using FluentValidation;
using TreeOfAKind.Application.Configuration;

namespace TreeOfAKind.Application.Command.Trees.TreeAdministration.CreateTree
{
    public class CreateTreeCommandValidator : AbstractValidator<CreateTreeCommand>
    {
        public CreateTreeCommandValidator()
        {
            RuleFor(x => x.TreeName)
                .NotNull()
                .NotEmpty()
                .MaximumLength(StringLengths.Short)
                .WithMessage($"{nameof(CreateTreeCommand.TreeName)} is longer than maximum length {StringLengths.Short}");

            RuleFor(x => x.UserAuthId)
                .NotNull()
                .NotEmpty()
                .MaximumLength(StringLengths.AuthIdLength)
                .WithMessage($"{nameof(CreateTreeCommand.UserAuthId)} is invalid");
        }
    }
}