using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;

namespace TreeOfAKind.Application.Command.Trees.People.AddOrChangePersonPhoto
{
    public class AddOrChangePersonPhotoCommandValidator : AbstractValidator<AddOrChangePersonPhotoCommand>
    {
        private readonly IEnumerable<string> AcceptedMimeTypes = new string[]
        {
            "image/jpg",
            "image/jpeg",
            "image/png",
        };

        private bool IsAcceptedMimeType(string mimeType)
            => AcceptedMimeTypes.Any(
                accepted => string.Equals(mimeType, accepted, StringComparison.OrdinalIgnoreCase));

        public AddOrChangePersonPhotoCommandValidator()
        {
            RuleFor(x => x.Document)
                .NotEmpty();

            RuleFor(x => x.Document.ContentType)
                .Must(IsAcceptedMimeType);

            RuleFor(x => x.Document.Content)
                .NotEmpty();

            RuleFor(x => x.PersonId)
                .NotEmpty();
        }
    }
}
