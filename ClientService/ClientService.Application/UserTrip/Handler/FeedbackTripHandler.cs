using ClientService.Application.Common.Constants;
using ClientService.Application.Common.Enums;
using ClientService.Application.Common.Exceptions;
using ClientService.Application.Common.Models.Response;
using ClientService.Application.Services.CurrentUserService;
using ClientService.Application.Services.ExpoService;
using ClientService.Application.UserTrip.Command;
using ClientService.Domain.Common;
using ClientService.Domain.Common.Enums.Notification;
using ClientService.Domain.Entities;
using ClientService.Domain.Wrappers;
using ClientService.Infrastructure.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ClientService.Application.UserTrip.Handler
{
    public class FeedbackTripHandler : IRequestHandler<FeedbackTripRequest, Response<StatusResponse>>
    {

        private readonly ILogger<FeedbackTripHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IExpoService _expoService;

        public FeedbackTripHandler(
            ILogger<FeedbackTripHandler> logger, IUnitOfWork unitOfWork, ICurrentUserService currentUserService, IExpoService expoService)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _expoService = expoService;
        }
        public async Task<Response<StatusResponse>> Handle(FeedbackTripRequest request, CancellationToken cancellationToken)
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
                throw new ApiException(ResponseCode.TripErrorNotFound);
            }
            var currentAccount = await _currentUserService.GetCurrentAccount();
            if (!trip.Passenger.Id.Equals(currentAccount.Id))
            {
                throw new ApiException(ResponseCode.TripErrorInvalidAccess);
            }

            if (!TripStatus.Finished.Equals(trip.TripStatus))
            {
                throw new ApiException(ResponseCode.TripErrorInvalidStatus);
            }

            if (trip.FeedbackPoint != null)
            {
                throw new ApiException(ResponseCode.TripErrorExistedFeedback);
            }

            float point = request.Point;
            string content = request.Content;

            // Update average point
            Account grabber = trip.Grabber;
            float currentAvgPoint = grabber.averagePoint;
            if (currentAvgPoint == null)
            {
                currentAvgPoint = 0f;
            }

            var tripQuery2 = await _unitOfWork.TripRepository.GetAsync(
               expression: x => x.Grabber.Id == grabber.Id && x.FeedbackPoint != null,
               includeFunc: (query) => query.Include(trip => trip.StartStation)
               .Include(trip => trip.EndStation)
               .Include(trip => trip.Grabber)
               .Include(trip => trip.Passenger)
               .Include(trip => trip.Post));

            int noFeedbackedTrip = tripQuery2.Count();
            float newAvgPoint = (float)(Math.Round(((currentAvgPoint * noFeedbackedTrip + point) / (noFeedbackedTrip + 1)) * 10) / 10.0f);

            grabber.averagePoint = newAvgPoint;
            await _unitOfWork.AccountRepository.UpdateAsync(grabber);

            trip.FeedbackPoint = point;
            trip.FeedbackContent = content;
            trip.Passenger = null;
            trip.Grabber = null;
            await _unitOfWork.TripRepository.UpdateAsync(trip);
            var res = await _unitOfWork.SaveChangesAsync();

            if (res > 0)
            {
                var notifiedPerson = currentAccount.Id == trip.GrabberId ? trip.Passenger.ExponentPushToken : trip.Grabber.ExponentPushToken;
                _expoService.sendTo(notifiedPerson.Token, new Services.ExpoService.Notification()
                {
                    Title = NotificationConstant.Title.TRIP_FEEDBACK,
                    Body = String.Format(NotificationConstant.Body.TRIP_FEEDBACK, currentAccount.Id, trip.StartStation.Name, trip.EndStation.Name),
                    Action = NotificationAction.OpenTrip,
                    ReferenceId = trip.Id.ToString(),
                });
            }

            return new Response<StatusResponse>
            {
                Code = 0,
                Data = new StatusResponse(true)
            };
        }
    }
}
