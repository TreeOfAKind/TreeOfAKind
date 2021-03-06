﻿using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TreeOfAKind.Application.Configuration.Commands;
using TreeOfAKind.Application.Services;
using TreeOfAKind.Domain.Trees;
using TreeOfAKind.Domain.UserProfiles;
using TreeOfAKind.Domain.UserProfiles.Rules;

namespace TreeOfAKind.Application.Command.Trees.TreeAdministration.AddTreeOwner
{
    public class AddTreeOwnerCommandHandler : ICommandHandler<AddTreeOwnerCommand, Unit>
    {
        private readonly ITreeRepository _treeRepository;
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly IUserAuthIdProvider _userAuthIdProvider;
        private readonly IUserAuthIdUniquenessChecker _userAuthIdUniquenessChecker;

        public AddTreeOwnerCommandHandler(ITreeRepository treeRepository, IUserProfileRepository userProfileRepository,
            IUserAuthIdProvider userAuthIdProvider, IUserAuthIdUniquenessChecker userAuthIdUniquenessChecker)
        {
            _treeRepository = treeRepository;
            _userProfileRepository = userProfileRepository;
            _userAuthIdProvider = userAuthIdProvider;
            _userAuthIdUniquenessChecker = userAuthIdUniquenessChecker;
        }

        public async Task<Unit> Handle(AddTreeOwnerCommand request, CancellationToken cancellationToken)
        {
            var tree = await _treeRepository.GetByIdAsync(request.TreeId, cancellationToken);

            var authId = await _userAuthIdProvider.GetUserAuthId(new MailAddress(request.AddedPersonMailAddress), cancellationToken);
            var addedUserProfile = await _userProfileRepository.GetByUserAuthIdAsync(authId, cancellationToken);

            if (addedUserProfile is null)
            {
                addedUserProfile = UserProfile.CreateUserProfile(
                    authId, new MailAddress(request.AddedPersonMailAddress), null, null, null, _userAuthIdUniquenessChecker);

                await _userProfileRepository.AddAsync(addedUserProfile, cancellationToken);
            }

            addedUserProfile.UpdateContactEmailAddress(new MailAddress(request.AddedPersonMailAddress));

            var invitor = await _userProfileRepository.GetByUserAuthIdAsync(request.RequesterUserAuthId, cancellationToken);

            tree!.AddTreeOwner(addedUserProfile, invitor!);

            return Unit.Value;
        }
    }
}
