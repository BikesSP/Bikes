using ClientService.Application.Common.Models.Response;
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

        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(PaginationResponse<PostResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetAllPosts([FromQuery] GetAllPostsRequest request)
        {
            return Ok(await mediator.Send(request));
        }

        [HttpGet("{id}")]
        [Authorize]
        [ProducesResponseType(typeof(PaginationResponse<PostResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetPostById(int id)
        {
            GetPostByIdRequest request = new GetPostByIdRequest()
            {
                Id = id 
            };
            return Ok(await mediator.Send(request));
        }

        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(typeof(Response<PostResponse?>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CancelPost(int id)
        {
            CancelPostRequest request = new CancelPostRequest()
            {
                Id = id
            };
            return Ok(await mediator.Send(request));
        }

        [HttpGet("public/all")]
        [Authorize]
        [ProducesResponseType(typeof(PaginationResponse<PostResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetActivePosts([FromQuery] GetActivePostsRequest request)
        {
            return Ok(await mediator.Send(request));
        }

        [HttpGet("{id}/appliers")]
        [Authorize]
        [ProducesResponseType(typeof(PaginationResponse<UserProfileResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ApplyPost(int id, [FromQuery] GetApplicationsByPostIdRequest request)
        {
            request.setId(id);
            return Ok(await mediator.Send(request));
        }

        [HttpPost("{id}/appliers")]
        [Authorize]
        [ProducesResponseType(typeof(Response<BaseBoolResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ApplyPost(int id)
        {
            ApplyPostRequest request = new ApplyPostRequest() { Id = id }; 
            return Ok(await mediator.Send(request));
        }

        [HttpDelete("{id}/appliers")]
        [Authorize]
        [ProducesResponseType(typeof(Response<BaseBoolResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> RejectApplication(int id)
        {
            CancelApplicationRequest request = new CancelApplicationRequest()
            {
                PostId= id
            };
            return Ok(await mediator.Send(request));
        }

        [HttpPut("{postId}/appliers/{applierId}")]
        [Authorize]
        [ProducesResponseType(typeof(Response<BaseBoolResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AcceptApplication(int postId, string applierId)
        {
            AcceptApplicationRequest request = new AcceptApplicationRequest()
            {
                ApplierId= applierId,
                PostId= postId
            };
            return Ok(await mediator.Send(request));
        }

        [HttpDelete("{postId}/appliers/{applierId}")]
        [Authorize]
        [ProducesResponseType(typeof(Response<BaseBoolResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> RejectApplication(int postId, string applierId)
        {
            RejectApplicationRequest request = new RejectApplicationRequest()
            {
                ApplierId = applierId,
                PostId = postId
            };
            return Ok(await mediator.Send(request));
        }
    }
}
