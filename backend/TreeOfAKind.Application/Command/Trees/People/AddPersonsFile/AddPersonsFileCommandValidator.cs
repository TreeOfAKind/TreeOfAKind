using System.Collections.Generic;
using FluentValidation;

namespace TreeOfAKind.Application.Command.Trees.People.AddPersonsFile
{
    public class AddPersonsFileCommandValidator : AbstractValidator<AddPersonsFileCommand>
    {
        private readonly IEnumerable<string> AcceptedMimeTypes = new string[]
        {
            "image/jpeg",
            "image/png",
            "application/pdf"
        };

        public AddPersonsFileCommandValidator()
        {
            RuleFor(x => x.Document)
                .NotEmpty()
                .SetValidator(new DocumentValidator(AcceptedMimeTypes));

            RuleFor(x => x.PersonId)
                .NotEmpty();
        }
    }
}
