using ClientService.Application.Auth.Command;
using ClientService.Application.Auth.Model;
using ClientService.Domain.Wrappers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace ClientService.API.Controllers
{
    [ApiController]
    [Route("/api/v1/auth")]
    public class AuthController : ApiControllerBase
    {
        public AuthController(IMediator mediator, ILogger<AuthController> logger) : base(mediator, logger)
        {
        }

        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(Response<TokenResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            return Ok(await mediator.Send(request));
        }

        [HttpPost("refresh")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(Response<TokenResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            return Ok(await mediator.Send(request));
        }

        [HttpPost("google-login")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(Response<TokenResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> LoginWithGoogle([FromBody] LoginWithGoogleRequest request)
        {
            return Ok(await mediator.Send(request));
        }
    }

}
