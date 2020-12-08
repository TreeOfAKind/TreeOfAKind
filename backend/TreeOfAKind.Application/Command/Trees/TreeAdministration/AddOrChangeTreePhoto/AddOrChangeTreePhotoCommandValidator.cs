using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;

namespace TreeOfAKind.Application.Command.Trees.TreeAdministration.AddOrChangeTreePhoto
{
    public class AddOrChangeTreePhotoCommandValidator : AbstractValidator<AddOrChangeTreePhotoCommand>
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

        public AddOrChangeTreePhotoCommandValidator()
        {
            RuleFor(x => x.Document)
                .NotEmpty();

            RuleFor(x => x.Document.ContentType)
                .Must(IsAcceptedMimeType);

            RuleFor(x => x.Document.Content)
                .NotEmpty();
        }
    }
}
