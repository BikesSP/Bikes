using ClientService.Application.Common.Constants;
using ClientService.Application.Common.Enums;
using ClientService.Application.Common.Exceptions;
using ClientService.Application.Common.Models.Response;
using ClientService.Application.Services.CurrentUserService;
using ClientService.Application.Services.ExpoService;
using ClientService.Application.UserPost.Handler;
using ClientService.Application.UserTrip.Command;
using ClientService.Domain.Common;
using ClientService.Domain.Common.Enums.Notification;
using ClientService.Domain.Entities;
using ClientService.Domain.Wrappers;
using ClientService.Infrastructure.Repositories;
using Google.Protobuf.WellKnownTypes;
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
    public class CancelTripHandler : IRequestHandler<CancelTripRequest, Response<StatusResponse>>
    {
        private readonly ILogger<CancelTripHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IExpoService _expoService;

        public CancelTripHandler(
            ILogger<CancelTripHandler> logger, IUnitOfWork unitOfWork, ICurrentUserService currentUserService, IExpoService expoService)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _expoService = expoService;
        }
        public async Task<Response<StatusResponse>> Handle(CancelTripRequest request, CancellationToken cancellationToken)
        {
            var tripQuery = await _unitOfWork.TripRepository.GetAsync(
                expression: x => x.Id == request.Id,
                includeFunc: (query) => query.Include(trip => trip.StartStation)
                .Include(trip => trip.EndStation)
                .Include(trip => trip.Grabber).ThenInclude(x => x.ExponentPushToken)
                .Include(trip => trip.Passenger).ThenInclude(x => x.ExponentPushToken)
                .Include(trip => trip.Post));
            var trip = tripQuery.FirstOrDefault();
            if (trip == null)
            {
                throw new ApiException(ResponseCode.TripErrorNotFound);
            }

            Account currentUser = await _currentUserService.GetCurrentAccount();

            Account grabber = trip.Grabber;
            Account passenger = trip.Passenger;
            if (!grabber.Id.Equals(currentUser.Id) && !passenger.Id.Equals(currentUser.Id))
            {
                throw new ApiException(ResponseCode.TripErrorInvalidAccess);
            }

            if (!TripStatus.Created.Equals(trip.TripStatus))
            {
                throw new ApiException(ResponseCode.TripErrorInvalidStatus);
            }

            DateTimeOffset startTime = trip.Post.StartTime;
            if (startTime.Subtract(DateTimeOffset.UtcNow).TotalSeconds < 30.60)
            {
                throw new ApiException(ResponseCode.TripErrorCannotCancelTrip);
            }

            trip.TripStatus = TripStatus.Canceled;
            trip.CancelAt = DateTimeOffset.UtcNow;
            trip.Passenger = null;
            trip.Grabber = null;
            await _unitOfWork.TripRepository.UpdateAsync(trip);
            var res = await _unitOfWork.SaveChangesAsync();

            if (res > 0)
            {
                var notifiedPerson = currentUser.Id == trip.GrabberId ? trip.Passenger : trip.Grabber;
                _expoService.sendTo(notifiedPerson?.ExponentPushToken?.Token, new Services.ExpoService.Notification()
                {
                    Title = NotificationConstant.Title.TRIP_CANCELED,
                    Body = String.Format(NotificationConstant.Body.TRIP_CANCELED, currentUser.Id),
                    Action = NotificationAction.OpenTrip,
                    ReferenceId = trip.Id.ToString(),
                });
            }

            return new Response<StatusResponse>
            {
                Code = 0,
                Data = new StatusResponse(true)
            };

            //Add Noti
        }
    }
}
