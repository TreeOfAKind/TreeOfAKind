using System.Collections.Generic;
using FluentValidation;

namespace TreeOfAKind.Application.Command.Trees.People.AddOrChangePersonsPhoto
{
    public class AddOrChangePersonsPhotoCommandValidator : AbstractValidator<AddOrChangePersonsPhotoCommand>
    {
        private readonly IEnumerable<string> AcceptedMimeTypes = new string[]
        {
            "image/jpg",
            "image/jpeg",
            "image/png",
        };

        public AddOrChangePersonsPhotoCommandValidator()
        {
            RuleFor(x => x.Document)
                .NotEmpty()
                .SetValidator(new DocumentValidator(AcceptedMimeTypes));

            RuleFor(x => x.PersonId)
                .NotEmpty();
        }
    }
}
