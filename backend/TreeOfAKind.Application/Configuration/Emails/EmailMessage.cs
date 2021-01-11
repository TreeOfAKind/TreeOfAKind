namespace TreeOfAKind.Application.Configuration.Emails
{
    public struct EmailMessage
    {
        public string To { get; }
        public string TreeName { get; }
        public string SenderName { get; }

        public EmailMessage(
            string to,
            string treeName, string senderName)
        {
            this.To = to;
            TreeName = treeName;
            SenderName = senderName;
        }
    }
}
