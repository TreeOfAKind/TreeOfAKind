using System;
using TreeOfAKind.Application.Configuration;

namespace TreeOfAKind.IntegrationTests
{
    public class ExecutionContextMock : IExecutionContextAccessor
    {
        public Guid CorrelationId { get; set; }

        public bool IsAvailable { get; set; }
    }
}