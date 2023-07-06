using ClientService.Application.Common.Models.Response;
using ClientService.Application.ExponentPushToken.Commands;
using ClientService.Application.UserPost.Command;
using ClientService.Application.UserPost.Model;
using ClientService.Domain.Wrappers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ClientService.API.Controllers
{
    [ApiController]
    [Route("/api/v1/expo-tokens")]
    public class ExponentPushTokenController : ApiControllerBase
    {
        public ExponentPushTokenController(IMediator mediator, ILogger<ExponentPushTokenController> logger) : base(mediator, logger)
        {
        }

        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(Response<BaseBoolResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreatePost([FromBody] AddExpoTokenRequest request)
        {
            return Ok(await mediator.Send(request));
        }

        [HttpPut]
        [Authorize]
        [ProducesResponseType(typeof(PaginationResponse<BaseBoolResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetAllPosts([FromBody] RemoveExpoTokenRequest request)
        {
            return Ok(await mediator.Send(request));
        }
    }
}
