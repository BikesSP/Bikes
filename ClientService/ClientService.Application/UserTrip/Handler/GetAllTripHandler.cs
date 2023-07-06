using ClientService.Application.Services.CurrentUserService;
using ClientService.Application.Trips.Handler;
using ClientService.Application.UserTrip.Model;
using ClientService.Application.UserTrip.Query;
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
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ClientService.Application.UserTrip.Handler
{
    public class GetAllTripHandler : IRequestHandler<GetAllUserTripRequest, PaginationResponse<UserTripResponse>>
    {
        private readonly ILogger<GetAllTripHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public GetAllTripHandler(
            ILogger<GetAllTripHandler> logger, IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<PaginationResponse<UserTripResponse>> Handle(GetAllUserTripRequest request, CancellationToken cancellationToken)
        {
            Account currentUser = await _currentUserService.GetCurrentAccount();
            request.UserId = currentUser.Id;
            var trips = await _unitOfWork.TripRepository.PaginationAsync(
                page: request.PageNumber,
                pageSize: request.PageSize,
                filter: request.GetExpressions(),
                orderBy: request.GetOrder(),
                includeFunc: query => query.Include(trip => trip.StartStation)
                .Include(trip => trip.EndStation)
                .Include(trip => trip.Grabber)
                .Include(trip => trip.Passenger)
                .Include(trip => trip.Post)
                .Include(trip => trip.Post.StartStation)
                .Include(trip => trip.Post.EndStation)
            );

            return new PaginationResponse<UserTripResponse>()
            {
                Code = 0,
                Data = new PaginationData<UserTripResponse>()
                {
                    Page = request.PageNumber,
                    PageSize = request.PageSize,
                    TotalSize = trips.Total,
                    TotalPage = (int?)((trips?.Total + (long)request.PageSize - 1) / (long)request.PageSize) ?? 0,
                    Items= trips.Data.ConvertAll(trips => new UserTripResponse(trips))
                }
            };
        }
    }
}
