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
using System.Linq.Dynamic.Core;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ClientService.Application.UserTrip.Handler
{
    public class StartTripHandler : IRequestHandler<StartTripRequest, Response<StatusResponse>>
    {
        private readonly ILogger<StartTripHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IExpoService _expoService;

        public StartTripHandler(
            ILogger<StartTripHandler> logger, IUnitOfWork unitOfWork, ICurrentUserService currentUserService, IExpoService expoService)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _expoService = expoService;
        }
        public async Task<Response<StatusResponse>> Handle(StartTripRequest request, CancellationToken cancellationToken)
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

            // Check current trip
            var SortColumn = "StartAt";
            var SortDir = SortDirection.Desc;

            var onGoingTrip = await _unitOfWork.TripRepository.GetAsync(
                expression: x => (x.Passenger.Id == currentUser.Id || x.Grabber.Id == currentUser.Id) && x.TripStatus.Equals(TripStatus.OnGoing),
                orderBy: GetOrder(SortColumn, SortDir));
            Trip? result = onGoingTrip.FirstOrDefault();
            if (result != null)
            {
                throw new ApiException(ResponseCode.TripErrorOngoingTrip);
            }

            trip.TripStatus = TripStatus.OnGoing;
            trip.StartAt = DateTimeOffset.UtcNow;

            trip.Passenger = null;
            trip.Grabber = null;
            await _unitOfWork.TripRepository.UpdateAsync(trip);
            var res = await _unitOfWork.SaveChangesAsync();

            if (res > 0)
            {
                var notifiedPerson = currentUser.Id == trip.GrabberId ? trip.Passenger.ExponentPushToken : trip.Grabber.ExponentPushToken;
                _expoService.sendTo(notifiedPerson.Token, new Services.ExpoService.Notification()
                {
                    Title = NotificationConstant.Title.TRIP_STARTED,
                    Body = String.Format(NotificationConstant.Body.TRIP_STARTED, currentUser.Id, trip.StartStation.Name, trip.EndStation.Name),
                    Action = NotificationAction.OpenTrip,
                    ReferenceId = trip.Id.ToString(),
                });
            }

            return new Response<StatusResponse>()
            {
                Code = 0,
                Data = new StatusResponse(true)
            };
        }

        public Func<IQueryable<Trip>, IOrderedQueryable<Trip>>? GetOrder(string SortColumn, SortDirection SortDir)
        {
            if (string.IsNullOrWhiteSpace(SortColumn)) return null;

            return query => query.OrderBy($"{SortColumn} {SortDir.ToString().ToLower()}");
        }
    }
}
