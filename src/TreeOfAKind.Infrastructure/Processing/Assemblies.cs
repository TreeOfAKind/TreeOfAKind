using System.Reflection;
using TreeOfAKind.Application.Configuration.Commands;

namespace TreeOfAKind.Infrastructure.Processing
{
    internal static class Assemblies
    {
        public static readonly Assembly Application = typeof(CommandBase).Assembly;
    }
}