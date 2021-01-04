using FluentValidation;
using TreeOfAKind.Application.Command.Trees.TreeAdministration.CreateTree;
using TreeOfAKind.Application.Configuration;

namespace TreeOfAKind.Application.Command.Trees.TreeAdministration.UpdateTreeName
{
    public class UpdateTreeCommandValidator : AbstractValidator<UpdateTreeNameCommand>
    {
        public UpdateTreeCommandValidator()
        {
            RuleFor(x => x.TreeName)
                .NotNull()
                .NotEmpty()
                .MaximumLength(StringLengths.Short)
                .WithMessage($"{nameof(CreateTreeCommand.TreeName)} is longer than maximum length {StringLengths.Short}");

            RuleFor(x => x.RequesterUserAuthId)
                .NotNull()
                .NotEmpty()
                .MaximumLength(StringLengths.AuthIdLength)
                .WithMessage($"{nameof(CreateTreeCommand.UserAuthId)} is invalid");

            RuleFor(x => x.TreeId)
                .NotEmpty();
        }
    }
}
