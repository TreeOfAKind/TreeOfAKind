using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using TreeOfAKind.Application.Ping;

namespace TreeOfAKind.API
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
        public IActionResult Index([FromRoute]string pingName)
        {
            return Ok(pingName ?? "Hello");
        }
    }
}