using ClientService.Application.Common.Models.Response;
using ClientService.Application.Stations.Command;
using ClientService.Application.Stations.Model;
using ClientService.Application.UserPost.Command;
using ClientService.Application.UserPost.Model;
using ClientService.Domain.Wrappers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Net;

namespace ClientService.API.Controllers
{
    [ApiController]
    [Route("/api/v1/stations")]
    public class StationController : ApiControllerBase
    {
        public StationController(IMediator mediator, ILogger<StationController> logger) : base(mediator, logger)
        {
        }

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(StationDetailResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        //[Authorize(Roles = "User")]
        public async Task<IActionResult> CreateStation([FromBody] CreateStationRequest request)
        {
            return Ok(await mediator.Send(request));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(StationDetailResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetStationDetail(int id)
        {
            GetStationDetailRequest request = new GetStationDetailRequest()
            {
                id = id
            };
            return Ok(await mediator.Send(request));
        }

        [HttpPut("{id}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(StationDetailResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        //[Authorize(Roles = "User")]
        public async Task<IActionResult> UpdateStation(int id, [FromBody] UpdateStationRequest request)
        {
            request.Id = id;
            return Ok(await mediator.Send(request));
        }

        [HttpGet]
        [ProducesResponseType(typeof(PaginationResponse<StationDetailResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetAllStations([FromQuery] GetAllStationsRequest request)
        {
            return Ok(await mediator.Send(request));
        }


        [HttpPut("{id}/status")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateStationStatus(int id, [FromBody] UpdateStationStatusRequest request)
        {
            request.Id = id;
            return Ok(await mediator.Send(request));
        }
    }
}
