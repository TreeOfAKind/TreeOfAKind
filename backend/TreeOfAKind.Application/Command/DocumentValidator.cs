using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using TreeOfAKind.Application.Configuration;

namespace TreeOfAKind.Application.Command
{
    public class DocumentValidator : AbstractValidator<Document>
    {
        private IEnumerable<string> AcceptedMimeTypes { get; }

        private bool IsAcceptedMimeType(string mimeType)
            => AcceptedMimeTypes.Any(
                accepted => string.Equals(mimeType, accepted, StringComparison.OrdinalIgnoreCase));

        public DocumentValidator(IEnumerable<string> acceptedMimeTypes)
        {
            AcceptedMimeTypes = acceptedMimeTypes;
            RuleFor(x => x.Content)
                .NotNull();

            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(StringLengths.Short);

            RuleFor(x => x.ContentType)
                .NotEmpty()
                .Must(IsAcceptedMimeType);
        }
    }
}
