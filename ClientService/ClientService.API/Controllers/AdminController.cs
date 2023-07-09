using ClientService.Application.Admin.Command;
using ClientService.Application.Stations.Command;
using ClientService.Application.Stations.Model;
using ClientService.Application.User.Model;
using ClientService.Application.Vehicles.Command;
using ClientService.Domain.Wrappers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ClientService.API.Controllers
{
    [ApiController]
    [Route("/api/v1")]
    public class AdminController : ApiControllerBase
    {
        public AdminController(IMediator mediator, ILogger<AdminController> logger) : base(mediator, logger)
        {
        }

        [HttpGet("vehicles/{id}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(VehicleResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetVehicle(string id)
        {
            GetVehicleRequest request = new GetVehicleRequest()
            {
                id = id
            };
            return Ok(await mediator.Send(request));
        }

        [HttpGet("vehicles")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(PaginationResponse<VehicleResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetAllVehicles([FromQuery] GetAllVehicesRequest request)
        {
            return Ok(await mediator.Send(request));
        }

        [HttpGet("accounts")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(PaginationResponse<UserProfileResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetAllAccounts([FromQuery] GetAllAccountsRequest request)
        {
            return Ok(await mediator.Send(request));
        }

        [HttpGet("accounts/{id}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(PaginationResponse<UserProfileResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetAccountDetail(string id)
        {
            GetAccountDetailRequest request = new GetAccountDetailRequest()
            {
                Id = id
            };
            return Ok(await mediator.Send(request));
        }

        [HttpPut("accounts/{id}")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateAccountStatus(string id, [FromQuery] string status)
        {
            UpdateAccountStatusRequest request = new UpdateAccountStatusRequest
            {
                Id = id,
                Status = status
            };
            return Ok(await mediator.Send(request));
        }
    }
}
