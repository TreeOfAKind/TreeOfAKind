using FluentValidation;
using TreeOfAKind.Application.Configuration;

namespace TreeOfAKind.Application.Command.UserProfiles.CreateOrUpdateUserProfile
{
    public class CreateOrUpdateUserProfileCommandValidator : AbstractValidator<CreateOrUpdateUserProfileCommand>
    {
        public CreateOrUpdateUserProfileCommandValidator()
        { 
            RuleFor(x => x.FirstName)
                .MaximumLength(StringLengths.Short)
                .WithMessage($"FirstName is longer than maximum length {StringLengths.Short}");
            
            RuleFor(x => x.LastName)
                .MaximumLength(StringLengths.Short)
                .WithMessage($"LastName is longer than maximum length {StringLengths.Short}");
            
            RuleFor(x => x.UserAuthId)
                .NotNull()
                .NotEmpty()
                .MaximumLength(StringLengths.AuthIdLength)
                .WithMessage($"UserAuthId is invalid");
        }
    }
}