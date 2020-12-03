using MediatR;

namespace TreeOfAKind.Application.Configuration.Commands
{
    public interface ICommandHandler<in TCommand, TResult> :
        IRequestHandler<TCommand, TResult> where TCommand : ICommand<TResult>
    {

    }
}