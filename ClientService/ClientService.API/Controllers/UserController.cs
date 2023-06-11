using ClientService.Application.Auth.Command;
using ClientService.Application.Auth.Model;
using ClientService.Domain.Wrappers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace ClientService.API.Controllers
{
    [ApiController]
    [Route("/api/v1/users")]
    public class UserController : ApiControllerBase
    {
        public UserController(IHttpContextAccessor httpContextAccessor, IWebHostEnvironment webHostEnvironment, IMediator mediator, ILogger<UserController> logger) : base(httpContextAccessor, webHostEnvironment, mediator, logger)
        {
        }

        [HttpGet("me")]
        [Authorize]
        [ProducesResponseType(typeof(Response<UserProfileResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> getCurrentUserProfile()
        {
            var email = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            Console.WriteLine(email);


            return Ok(await mediator.Send(new GetCurrentUserRequest()
            {
                Email = email
            }));
        }

        [HttpPost("me")]
        [Authorize]
        [ProducesResponseType(typeof(Response<TokenResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> LoginWithGoogle([FromBody] UpdateUserProfileRequest request)
        {
            var email = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

            request.Email = email;
            return Ok(await mediator.Send(request));
        }
    }
}
