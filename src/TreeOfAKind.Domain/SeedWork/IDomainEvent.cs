using System;
using MediatR;

namespace TreeOfAKind.Domain.SeedWork
{
    public interface IDomainEvent : INotification
    {
        DateTime OccurredOn { get; }
    }
}