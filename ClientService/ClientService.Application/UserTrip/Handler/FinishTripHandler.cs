using ClientService.Application.Common.Enums;
using ClientService.Application.Common.Exceptions;
using ClientService.Application.Common.Models.Response;
using ClientService.Application.Services.CurrentUserService;
using ClientService.Application.UserTrip.Command;
using ClientService.Domain.Common;
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

namespace ClientService.Application.UserTrip.Handler
{
    public class FinishTripHandler : IRequestHandler<FinishTripRequest, Response<StatusResponse>>
    {
        private readonly ILogger<FinishTripHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public FinishTripHandler(
            ILogger<FinishTripHandler> logger, IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }
        public async Task<Response<StatusResponse>> Handle(FinishTripRequest request, CancellationToken cancellationToken)
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

            if (!TripStatus.OnGoing.Equals(trip.TripStatus))
            {
                throw new ApiException(ResponseCode.TripErrorInvalidStatus);
            }

            trip.TripStatus = TripStatus.Finished;
            trip.FinishAt = DateTimeOffset.UtcNow;
            trip.Passenger = null;
            trip.Grabber = null;
            await _unitOfWork.TripRepository.UpdateAsync(trip);
            await _unitOfWork.SaveChangesAsync();
            return new Response<StatusResponse>
            {
                Code = 0,
                Data = new StatusResponse(true)
            };
        }
    }
}
