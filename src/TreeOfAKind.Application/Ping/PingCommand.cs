using TreeOfAKind.Application.Configuration.Commands;

namespace TreeOfAKind.Application.Ping
{
    public class PingCommand : CommandBase
    {
        public string PingName { get; }

        public PingCommand(string pingName)
        {
            PingName = pingName;
        }
    }
}