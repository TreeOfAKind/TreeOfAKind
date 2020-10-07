using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using TreeOfAKind.Application.Ping;

namespace TreeOfAKind.API.Tree
{
    [Route("api/ping")]
    public class PingController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IMediator _mediator;

        public PingController(IWebHostEnvironment webHostEnvironment, IMediator mediator)
        {
            this._webHostEnvironment = webHostEnvironment;
            this._mediator = mediator;
        }

        [HttpGet]
        [Route("{pingName}")]
        [Authorize]
        public async Task<IActionResult> Index([FromRoute]string pingName)
        {
            if (!_webHostEnvironment.IsDevelopment())
            {
                return await Task.FromResult(NotFound());
            }

            await _mediator.Send(new PingCommand(pingName));
            
            return await Task.FromResult(Ok());
        }
    }
}