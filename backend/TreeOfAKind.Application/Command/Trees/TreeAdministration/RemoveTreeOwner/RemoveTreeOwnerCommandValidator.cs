using FluentValidation;
using TreeOfAKind.Domain.UserProfiles;

namespace TreeOfAKind.Application.Command.Trees.TreeAdministration.RemoveTreeOwner
{
    public class RemoveTreeOwnerCommandValidator : AbstractValidator<RemoveTreeOwnerCommand>
    {
        private readonly IUserProfileRepository _userProfileRepository;

        public RemoveTreeOwnerCommandValidator(IUserProfileRepository userProfileRepository)
        {
            _userProfileRepository = userProfileRepository;
            
            RuleFor(x => x.UserToRemoveId)
                .NotEmpty()
                .MustAsync(async (x,c) => await _userProfileRepository.GetByIdAsync(x) is {})
                .WithMessage($"{nameof(RemoveTreeOwnerCommand.UserToRemoveId)} is not valid.");
        }
    }
}