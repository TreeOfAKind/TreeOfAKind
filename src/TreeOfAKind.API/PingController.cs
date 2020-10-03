using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace TreeOfAKind.API.Tree
{
    [Route("api/ping")]
    public class PingController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public PingController(IWebHostEnvironment webHostEnvironment)
        {
            this._webHostEnvironment = webHostEnvironment;
        }
        [HttpGet]
        [Route("")]
        [Authorize]
        public async Task<IActionResult> Index()
            => _webHostEnvironment.IsDevelopment() ?
                await Task.FromResult(Ok())
                :
                await Task.FromResult(NotFound()) as IActionResult;
    }
}