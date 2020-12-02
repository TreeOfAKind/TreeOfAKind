using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using TreeOfAKind.API.SeedWork;
using TreeOfAKind.Application.Configuration.Validation;
using TreeOfAKind.Application.Ping;

namespace TreeOfAKind.API
{
    public sealed class PingQueryRequest
    {
        public int? ErrorCode { get; set; }
        public PingQueryResponse PingQueryResponse { get; set; }
    }

    public sealed class PingQueryResponse
    {
        public string SomeString { get; set; }
        public DateTime SomeDateTime { get; set; }
        public double SomeDouble { get; set; }
    }
    

    [Route("api/ping")]
    public class PingController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IMediator _mediator;
        private readonly IConfiguration _configuration;

        public PingController(IWebHostEnvironment webHostEnvironment, IMediator mediator, IConfiguration configuration)
        {
            this._webHostEnvironment = webHostEnvironment;
            this._mediator = mediator;
            _configuration = configuration;
        }

        /// <summary>
        /// Returns desired error code or object.
        /// </summary>
        /// <remarks>
        /// Returns error code specified in request. <br /> 
        /// If no error code is specified returns pingQueryResponse from request. <br /> 
        /// If neither one is specified returns response with predefined data. <br /> 
        /// NOT AVAILABLE IN PRODUCTION <br /> <br /> 
        /// Sample request:
        ///
        ///     POST /api/ping/pingQuery
        ///     {
        ///        "errorCode": 400,
        ///     }
        ///
        ///     will return http 400
        ///
        /// Sample request
        /// 
        ///     POST /api/ping/pingQuery
        ///     {
        ///     }
        ///
        ///     will return response with predefined data.
        /// 
        /// </remarks>
        /// <param name="request">Object specifying what error code or value should ping return</param>
        /// <response code="200">Returns response object from request or if request doesnt specify one predefined response object</response>
        [HttpPost]
        [Route("pingQuery")]
        [ProducesResponseType(typeof(PingQueryResponse), StatusCodes.Status200OK)]
        public IActionResult pingQuery([FromBody] PingQueryRequest request)
        {
            if (request.ErrorCode.HasValue)
            {
                if (request.ErrorCode.Value == StatusCodes.Status400BadRequest)
                {
                    throw new InvalidCommandException("No ziomek 400", "Nie prość o 400 w request");
                }
                return StatusCode(request.ErrorCode.Value);
            }
            
            if (request.PingQueryResponse is { })
            {
                return Ok(request.PingQueryResponse);
            }

            return Ok(new PingQueryResponse()
            {
                SomeString = "To jest string",
                SomeDouble = 12.9,
                SomeDateTime = DateTime.Now,
            });
        }
        

        /// <summary>
        /// Returns name of deployment service
        /// </summary>
        /// <response code="200">Name of deployment service.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Route("deploymentservice")]
        public IActionResult Index()
        {
            return Ok(_configuration["deployment:service"]);
        }
    }
}
