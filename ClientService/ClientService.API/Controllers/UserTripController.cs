using ClientService.Application.Common.Enums;
using ClientService.Application.Common.Exceptions;
using ClientService.Application.Common.Models.Response;
using ClientService.Application.UserTrip.Command;
using ClientService.Application.UserTrip.Model;
using ClientService.Application.UserTrip.Query;
using ClientService.Domain.Common;
using ClientService.Domain.Wrappers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;
using System.Threading.Channels;

namespace ClientService.API.Controllers
{
    [ApiController]
    [Route("/api/v1/trips")]
    public class UserTripController : ApiControllerBase
    {
        protected UserTripController(IMediator mediator, ILogger logger) : base(mediator, logger)
        {
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<PaginationResponse<UserTripResponse>>> GetAllUserTrips([FromQuery] GetAllUserTripRequest request)
        {
            return await mediator.Send(request);
        }

        [HttpGet("ongoing")]
        [Authorize]
        public async Task<ActionResult<Response<UserTripDetailResponse>>> GetOngoingtrip()
        {
            GetOnGoingTripRequest request = new GetOnGoingTripRequest();
            return await mediator.Send(request);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Response<UserTripDetailResponse>>> GetById([FromRoute] long id)
        {
            var request = new GetTripRequest();
            request.Id = id;
            return await mediator.Send(request);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<Response<StatusResponse>>> UpdateTrip([FromRoute] long id, [FromQuery] TripAction action)
        {
            switch (action)
            {
                case TripAction.Cancel:
                    CancelTripRequest request = new CancelTripRequest();
                    request.Id = id;
                    return await mediator.Send(request);
                case TripAction.Start:
                    StartTripRequest startTripRequest = new StartTripRequest();
                    startTripRequest.Id = id;
                    return await mediator.Send(startTripRequest);
                case TripAction.Finish:
                    FinishTripRequest finishTripRequest = new FinishTripRequest();
                    finishTripRequest.Id = id;
                    return await mediator.Send(finishTripRequest);
            }
            throw new ApiException(ResponseCode.InvalidParam);
        }

        [HttpPost("{id}/feedbacks")]
        [Authorize]
        public async Task<ActionResult<Response<StatusResponse>>> FeedbackTrip([FromRoute] long id, [FromBody] FeedbackTripRequest request)
        {
            request.Id = id;
            return await mediator.Send(request);
        }
    }
}
