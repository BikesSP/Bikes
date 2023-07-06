using ClientService.Application.Common.Enums;
using ClientService.Application.Common.Extensions;
using ClientService.Application.Services.CurrentUserService;
using ClientService.Application.UserTrip.Model;
using ClientService.Application.UserTrip.Query;
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
    public class GetTripHandler : IRequestHandler<GetTripRequest, Response<UserTripDetailResponse>>
    {
        private readonly ILogger<GetTripHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public GetTripHandler(
            ILogger<GetTripHandler> logger, IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }
        public async Task<Response<UserTripDetailResponse>> Handle(GetTripRequest request, CancellationToken cancellationToken)
        {
            var tripQuery = await _unitOfWork.TripRepository.GetAsync(
                expression: x => x.Id == request.Id,
                includeFunc: (query) => query.Include(trip => trip.StartStation)
                .Include(trip => trip.EndStation)
                .Include(trip => trip.Grabber)
                .Include(trip => trip.Passenger)
                .Include(trip => trip.Post)
                .Include(trip => trip.Post.StartStation)
                .Include(trip => trip.Post.EndStation));
            var trip = tripQuery.FirstOrDefault();

            if (trip == null)
            {
                return new Response<UserTripDetailResponse>(code: (int)ResponseCode.TripErrorNotFound, message: ResponseCode.TripErrorNotFound.GetDescription());
            }
            return new Response<UserTripDetailResponse>()
            {
                Code = 0,
                Data = new UserTripDetailResponse(trip)
            };
        }
    }
}
