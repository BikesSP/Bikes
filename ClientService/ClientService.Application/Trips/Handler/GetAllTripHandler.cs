using ClientService.Application.Common.Models.Response;
using ClientService.Application.Services.CurrentUserService;
using ClientService.Application.Trips.Model;
using ClientService.Application.Trips.Query;
using ClientService.Application.UserPost.Handler;
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
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ClientService.Application.Trips.Handler
{
    public class GetAllTripHandler : IRequestHandler<GetAllTripRequest, PaginationResponse<TripResponse>>
    {
        private readonly ILogger<GetAllTripHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public GetAllTripHandler(
            ILogger<GetAllTripHandler> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }
        public async Task<PaginationResponse<TripResponse>> Handle(GetAllTripRequest request, CancellationToken cancellationToken)
        {
            var trips = await _unitOfWork.TripRepository.PaginationAsync(
                filter: request.GetExpressions(),
                orderBy: request.GetOrder(),
                includeFunc: query => query.Include(trip => trip.StartStation)
                .Include(trip => trip.EndStation)
                .Include(trip => trip.Grabber)
                .Include(trip => trip.Passenger)
                .Include(trip => trip.Post)
            );

            return new PaginationResponse<TripResponse>(code: 0,
                        data: new PaginationData<TripResponse>()
                        {
                            Page = request.PageNumber,
                            PageSize = request.PageSize,
                            TotalSize = trips.Total,
                            TotalPage = (int?)((trips?.Total + (long)request.PageSize - 1) / (long)request.PageSize) ?? 0,
                            Items = trips.Data.ConvertAll(trips => new TripResponse()
                            {
                                Id = trips.Id,
                                StartStationName = trips.StartStation.Name,
                                EndStationName = trips.EndStation.Name,
                                StartStationId = trips.StartStation.Id,
                                EndStationId = trips.EndStation.Id,
                                GrabberId = trips.Grabber.Id,
                                GrabberName = trips.Grabber.Name,
                                PassengerId = trips.Passenger.Id,
                                PassengerName = trips.Passenger.Name,
                                StartTime = trips.StartAt,
                                CancelTime = trips.CancelAt,
                                FeedbackContent = trips.FeedbackContent,
                                FeedbackPoint = trips.FeedbackPoint,
                                Status = trips.TripStatus,
                                PostedStartTime = trips.Post.StartTime
                            })
                        }
                    );
        }
    }
}
