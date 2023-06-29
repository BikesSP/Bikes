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

        public FeedbackTripHandler(
            ILogger<FeedbackTripHandler> logger, IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
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

            if (!trip.Passenger.Id.Equals(_currentUserService.GetCurrentAccount().Id))
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
