using ClientService.Application.Stations.Command;
using ClientService.Application.Stations.Model;
using ClientService.Application.User.Command;
using ClientService.Application.User.Model;
using ClientService.Application.UserPost.Command;
using ClientService.Application.UserPost.Model;
using ClientService.Domain.Wrappers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ClientService.API.Controllers
{
    [ApiController]
    [Route("/api/v1/posts")]
    public class PostController : ApiControllerBase
    {
        public PostController(IMediator mediator, ILogger<PostController> logger) : base(mediator, logger)
        {
        }

        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(Response<PostResponse?>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreatePost([FromBody] CreatePostRequest request)
        {
            return Ok(await mediator.Send(request));
        }

        [HttpGet("public")]
        [ProducesResponseType(typeof(Response<PostResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetActivePosts([FromQuery] GetActivePostsRequest request)
        {
            return Ok(await mediator.Send(request));
        }
    }
}
