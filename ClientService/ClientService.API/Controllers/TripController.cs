using ClientService.Application.Trips.Model;
using ClientService.Application.Trips.Query;
using ClientService.Domain.Wrappers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClientService.API.Controllers
{
    [ApiController]
    [Route("/api/v1/trips")]
    public class TripController : ApiControllerBase
    {

        protected TripController(IMediator mediator, ILogger logger) : base(mediator, logger)
        {
        }

        [HttpGet]
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<PaginationResponse<TripResponse>>> GetAllTrips([FromQuery]GetAllTripRequest request)
        {
            return await mediator.Send(request);
        }

        [HttpGet("{id}")]
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Response<TripDetailResponse>>> GetById([FromRoute] long Id)
        {
            var request = new GetTripDetailRequest();
            request.Id = Id;
            return await mediator.Send(request);
        }
    }
}
