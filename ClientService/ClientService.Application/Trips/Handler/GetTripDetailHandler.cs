using ClientService.Application.Common.Enums;
using ClientService.Application.Common.Extensions;
using ClientService.Application.Trips.Model;
using ClientService.Application.Trips.Query;
using ClientService.Application.UserPost.Model;
using ClientService.Domain.Entities;
using ClientService.Domain.Wrappers;
using ClientService.Infrastructure.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientService.Application.Trips.Handler
{
    public class GetTripDetailHandler : IRequestHandler<GetTripDetailRequest, Response<TripDetailResponse>>
    {
        private readonly ILogger<GetTripDetailHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public GetTripDetailHandler(
            ILogger<GetTripDetailHandler> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }
        public async Task<Response<TripDetailResponse>> Handle(GetTripDetailRequest request, CancellationToken cancellationToken)
        {
            var tripQuery = await _unitOfWork.TripRepository.GetAsync(
                expression: x => x.Id == request.Id,
                includeFunc: (query) => query.Include(trip => trip.StartStation)
                .Include(trip => trip.EndStation)
                .Include(trip => trip.Grabber)
                .Include(trip => trip.Passenger)
                .Include(trip => trip.Post));
            var trip = tripQuery.FirstOrDefault();

            if (trip == null)
            {
                return new Response<TripDetailResponse>(code: (int)ResponseCode.TripErrorNotFound, message: ResponseCode.TripErrorNotFound.GetDescription());
            }
            return new Response<TripDetailResponse>()
            {
                Code = 0,
                Data = new TripDetailResponse()
                {
                    Id = trip.Id,
                    StartStationName = trip.StartStation.Name,
                    EndStationName = trip.EndStation.Name,
                    StartStationId = trip.StartStation.Id,
                    EndStationId = trip.EndStation.Id,
                    GrabberId = trip.Grabber.Id,
                    GrabberName = trip.Grabber.Name,
                    PassengerId = trip.Passenger.Id,
                    PassengerName = trip.Passenger.Name,
                    StartTime = trip.StartAt,
                    CancelTime = trip.CancelAt,
                    FeedbackContent = trip.FeedbackContent,
                    FeedbackPoint = trip.FeedbackPoint,
                    Status = trip.TripStatus,
                    PostedStartTime = trip.Post.StartTime
                }
            };
        }
    }
}
