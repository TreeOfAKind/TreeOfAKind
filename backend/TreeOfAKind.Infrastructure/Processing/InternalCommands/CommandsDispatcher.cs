using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TreeOfAKind.Infrastructure.Database;

namespace TreeOfAKind.Infrastructure.Processing.InternalCommands
{
    public class CommandsDispatcher : ICommandsDispatcher
    {
        private readonly IMediator _mediator;
        private readonly TreesContext _treesContext;

        public CommandsDispatcher(
            IMediator mediator, 
            TreesContext treesContext)
        {
            this._mediator = mediator;
            this._treesContext = treesContext;
        }

        public async Task DispatchCommandAsync(Guid id)
        {
            var internalCommand = await this._treesContext.InternalCommands.SingleOrDefaultAsync(x => x.Id == id);
            
            Type type = Assemblies.Application.GetType(internalCommand.Type);
            dynamic command = JsonConvert.DeserializeObject(internalCommand.Data, type);
            
            internalCommand.ProcessedDate = DateTime.UtcNow;
            
            await this._mediator.Send(command);
        }
    }
}