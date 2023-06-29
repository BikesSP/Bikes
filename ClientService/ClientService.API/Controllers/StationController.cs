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

        [HttpGet("{stationId}")]
        [ProducesResponseType(typeof(StationDetailResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetStationDetail(int stationId)
        {
            GetStationDetailRequest request = new GetStationDetailRequest()
            {
                id = stationId
            };
            return Ok(await mediator.Send(request));
        }

        [HttpPut("{stationId}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(StationDetailResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        //[Authorize(Roles = "User")]
        public async Task<IActionResult> UpdateStation(int stationId, [FromBody] UpdateStationRequest request)
        {
            request.Id = stationId;
            return Ok(await mediator.Send(request));
        }

        [HttpGet]
        [ProducesResponseType(typeof(PaginationResponse<StationDetailResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetAllStations([FromQuery] GetAllStationsRequest request)
        {
            return Ok(await mediator.Send(request));
        }


        [HttpPut("{stationId}/status")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateStationStatus(int stationId, [FromBody] UpdateStationStatusRequest request)
        {
            request.Id = stationId;
            return Ok(await mediator.Send(request));
        }
    }
}
