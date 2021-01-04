using System.Collections.Generic;
using FluentValidation;

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
                .SetValidator(new DocumentValidator(AcceptedMimeTypes));
        }
    }
}
