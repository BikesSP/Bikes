using ClientService.Application.Common.Enums;
using ClientService.Application.Common.Exceptions;
using ClientService.Application.Common.Models.Response;
using ClientService.Application.Services.CurrentUserService;
using ClientService.Application.UserPost.Handler;
using ClientService.Application.UserTrip.Command;
using ClientService.Domain.Common;
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

        public CancelTripHandler(
            ILogger<CancelTripHandler> logger, IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }
        public async Task<Response<StatusResponse>> Handle(CancelTripRequest request, CancellationToken cancellationToken)
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

            DateTimeOffset startTime = trip.Post.StartTime;
            if (startTime.Subtract(DateTimeOffset.Now).TotalSeconds < 30.60)
            {
                throw new ApiException(ResponseCode.TripErrorCannotCancelTrip);
            }

            trip.TripStatus = TripStatus.Canceled;
            trip.CancelAt = DateTimeOffset.Now;
            await _unitOfWork.TripRepository.UpdateAsync(trip);
            await _unitOfWork.SaveChangesAsync();
            return new Response<StatusResponse>
            {
                Code = 0,
                Data = new StatusResponse(true)
            };

            //Add Noti
        }
    }
}
