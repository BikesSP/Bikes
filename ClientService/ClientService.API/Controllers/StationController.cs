using ClientService.Application.Common.Models.Response;
using ClientService.Application.Stations.Command;
using ClientService.Application.Stations.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace ClientService.API.Controllers
{
    [ApiController]
    [Route("/api/v1/stations")]

    public class StationController : ApiControllerBase
    {
        [HttpPost]
        //[Authorize(Roles = "User")]
        public async Task<ActionResult<StationDetailResponse>> Create(CreateStationRequest request)
        {
            return await Mediator.Send(request);
        }

    }
}
