using System;
using MediatR;

namespace TreeOfAKind.Application.Configuration.Commands
{
    public interface ICommand : ICommand<Unit>
    {
        
    }

    public interface ICommand<out TResult> : IRequest<TResult>
    {
        Guid Id { get; }
    }
}