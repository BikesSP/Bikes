using ClientService.Application.Trips.Model;
using ClientService.Application.Trips.Query;
using ClientService.Domain.Wrappers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClientService.API.Controllers
{
    [ApiController]
    [Route("/api/cms/v1/trips")]
    public class TripController : ApiControllerBase
    {

        public TripController(IMediator mediator, ILogger<TripController> logger) : base(mediator, logger)
        {
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<PaginationResponse<TripResponse>>> GetAllTrips([FromQuery]GetAllTripRequest request)
        {
            return await mediator.Send(request);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Response<TripDetailResponse>>> GetById([FromRoute] long id)
        {
            var request = new GetTripDetailRequest();
            request.Id = id;
            return await mediator.Send(request);
        }
    }
}
