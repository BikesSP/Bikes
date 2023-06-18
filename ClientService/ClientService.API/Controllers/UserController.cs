using ClientService.Application.Auth.Model;
using ClientService.Application.Common.Models.Response;
using ClientService.Application.User.Command;
using ClientService.Application.User.Model;
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
        public UserController(IMediator mediator, ILogger<UserController> logger) : base(mediator, logger)
        {
        }

        [HttpGet("me")]
        [Authorize]
        [ProducesResponseType(typeof(Response<UserProfileResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> getCurrentUserProfile()
        {
            return Ok(await mediator.Send(new GetCurrentUserRequest()));
        }

        [HttpPut("me")]
        [Authorize]
        [ProducesResponseType(typeof(Response<TokenResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> LoginWithGoogle([FromBody] UpdateUserProfileRequest request)
        {
            return Ok(await mediator.Send(request));
        }

        [HttpGet("me/vehicle")]
        [Authorize]
        [ProducesResponseType(typeof(Response<VehicleResponse?>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetCurrentUserVehicle()
        {
            return Ok(await mediator.Send(new GetCurrentUserVehicleRequest()));
        }

        [HttpPut("me/vehicle")]
        [Authorize]
        [ProducesResponseType(typeof(Response<VehicleResponse?>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateCurrentUserVehicle([FromBody] UpdateVehicleRequest request)
        {
            return Ok(await mediator.Send(request));
        }

        [HttpPut("me/vehicle/{id}/status")]
        [ProducesResponseType(typeof(Response<BaseBoolResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateVehicleStatus(string id, [FromBody] UpdateVehicleStatusRequest request)
        {
            request.Id = id;
            return Ok(await mediator.Send(request));
        }
    }
}
