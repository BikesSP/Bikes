using ClientService.Application.Common.Models.Response;
using ClientService.Application.Stations.Command;
using ClientService.Application.Stations.Model;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace ClientService.API.Controllers
{
    [ApiController]
    [Route("/api/v1/stations")]

    public class StationController : ApiControllerBase
    {
        public StationController(IHttpContextAccessor httpContextAccessor, IWebHostEnvironment webHostEnvironment, IMediator mediator, ILogger<StationController> logger) : base(httpContextAccessor, webHostEnvironment, mediator, logger)
        {
        }

        [HttpPost]
        //[Authorize(Roles = "User")]
        [Authorize]
        public async Task<ActionResult<StationDetailResponse>> Create(CreateStationRequest request)
        {
            return await mediator.Send(request);
        }

    }
}
