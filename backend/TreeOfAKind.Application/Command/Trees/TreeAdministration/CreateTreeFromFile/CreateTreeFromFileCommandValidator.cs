using System.Collections.Generic;
using FluentValidation;
using TreeOfAKind.Application.Command.Trees.TreeAdministration.CreateTree;
using TreeOfAKind.Application.Configuration;

namespace TreeOfAKind.Application.Command.Trees.TreeAdministration.CreateTreeFromFile
{
    public class CreateTreeFromFileCommandValidator : AbstractValidator<CreateTreeFromFileCommand>
    {
        private readonly IEnumerable<string> AcceptedMimeTypes = new string[]
        {
            "text/xml",
        };

        public CreateTreeFromFileCommandValidator()
        {
            RuleFor(x => x.Document)
                .NotEmpty()
                .SetValidator(new DocumentValidator(AcceptedMimeTypes))
                .WithMessage("File invalid");

            RuleFor(x => x.TreeName)
                .NotNull()
                .NotEmpty()
                .MaximumLength(StringLengths.Short)
                .WithMessage($"{nameof(CreateTreeFromFileCommand.TreeName)} is longer than maximum length {StringLengths.Short}");

            RuleFor(x => x.UserAuthId)
                .NotNull()
                .NotEmpty()
                .MaximumLength(StringLengths.AuthIdLength)
                .WithMessage($"{nameof(CreateTreeFromFileCommand.UserAuthId)} is invalid");
        }
    }
}
