using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TreeOfAKind.Application.Configuration.Emails;
using TreeOfAKind.Domain.Trees;
using TreeOfAKind.Domain.Trees.Events;
using TreeOfAKind.Domain.UserProfiles;

namespace TreeOfAKind.Application.Command.Trees.TreeAdministration.AddTreeOwner
{
    public class TreeOwnerAddedEventHandler : INotificationHandler<TreeOwnerAddedEvent>
    {
        private readonly IEmailSender _emailSender;
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly ITreeRepository _treeRepository;

        public TreeOwnerAddedEventHandler(IEmailSender emailSender, IUserProfileRepository userProfileRepository, ITreeRepository treeRepository)
        {
            _emailSender = emailSender;
            _userProfileRepository = userProfileRepository;
            _treeRepository = treeRepository;
        }

        public async Task Handle(TreeOwnerAddedEvent notification, CancellationToken cancellationToken)
        {
            var invited = notification.Invited;
            var invitor = notification.Invitor;
            var tree = await _treeRepository.GetByIdAsync(notification.Tree, cancellationToken);
            var senderName = (invitor!.FirstName + " " + invitor!.LastName).Trim();
            senderName = string.IsNullOrWhiteSpace(senderName) ? invitor.ContactEmailAddress!.Address : senderName;
            var emailMessage = new EmailMessage(invited!.ContactEmailAddress!.Address, tree!.Name, senderName);
            await _emailSender.SendEmailAsync(emailMessage);
        }
    }
}
