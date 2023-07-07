using ClientService.Application.Media.Command;
using ClientService.Application.Media.Model;
using ClientService.Application.UserTrip.Model;
using ClientService.Application.UserTrip.Query;
using ClientService.Domain.Wrappers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClientService.API.Controllers
{
    [ApiController]
    [Route("/api/v1/medias")]
    public class MediaController : ApiControllerBase
    {
        public MediaController(IMediator mediator, ILogger<MediaController> logger) : base(mediator, logger)
        {
        }

        [HttpPost()]
        [Authorize]
        public async Task<ActionResult<Response<UploadFileResponse>>> UploadFile([FromForm]UploadFileRequest request)
        {
            return await mediator.Send(request);
        }
    }
}
