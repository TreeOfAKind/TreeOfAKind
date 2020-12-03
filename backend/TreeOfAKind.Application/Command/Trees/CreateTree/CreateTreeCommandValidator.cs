using FluentValidation;
using TreeOfAKind.Application.Configuration;
using TreeOfAKind.Domain.Trees;

namespace TreeOfAKind.Application.Command.Trees.CreateTree
{
    public class CreateTreeCommandValidator : AbstractValidator<CreateTreeCommand>
    {
        public CreateTreeCommandValidator()
        {
            RuleFor(x => x.TreeName)
                .NotNull()
                .NotEmpty()
                .MaximumLength(StringLengths.VeryShort)
                .WithMessage($"{nameof(CreateTreeCommand.TreeName)} is longer than maximum length {StringLengths.VeryShort}");

            RuleFor(x => x.UserAuthId)
                .NotNull()
                .NotEmpty()
                .MaximumLength(StringLengths.AuthIdLength)
                .WithMessage($"{nameof(CreateTreeCommand.UserAuthId)} is invalid");
        }
    }
}