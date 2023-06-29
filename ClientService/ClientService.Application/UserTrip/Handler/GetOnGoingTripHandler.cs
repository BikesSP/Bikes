using ClientService.Application.Common.Enums;
using ClientService.Application.Services.CurrentUserService;
using ClientService.Application.UserTrip.Model;
using ClientService.Application.UserTrip.Query;
using ClientService.Domain.Common;
using ClientService.Domain.Entities;
using ClientService.Domain.Wrappers;
using ClientService.Infrastructure.Repositories;
using ClientService.Infrastructure.Repositories.TripRepository;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace ClientService.Application.UserTrip.Handler
{
    public class GetOnGoingTripHandler : IRequestHandler<GetOnGoingTripRequest, Response<UserTripDetailResponse>>
    {
        private readonly ILogger<GetOnGoingTripHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public GetOnGoingTripHandler(
            ILogger<GetOnGoingTripHandler> logger, IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }
        public async Task<Response<UserTripDetailResponse>> Handle(
            GetOnGoingTripRequest request, 
            CancellationToken cancellationToken)
        {
            var SortColumn = "StartAt";
            var SortDir = SortDirection.Desc;
            Account currentUser = await _currentUserService.GetCurrentAccount();
           
            var trips = await _unitOfWork.TripRepository.GetAsync(
                expression: x => (x.Passenger.Id == currentUser.Id || x.Grabber.Id == currentUser.Id) && x.TripStatus.Equals(TripStatus.OnGoing),
                orderBy: GetOrder(SortColumn, SortDir));
            Trip? result = trips.FirstOrDefault();
            if (result == null)
            {
                return null;
            } else
            {
                return new Response<UserTripDetailResponse>()
                {
                    Code = 0,
                    Data = new UserTripDetailResponse(result)
                };
            }
            
            
        }

        public Func<IQueryable<Trip>, IOrderedQueryable<Trip>>? GetOrder(string SortColumn, SortDirection SortDir)
        {
            if (string.IsNullOrWhiteSpace(SortColumn)) return null;

            return query => query.OrderBy($"{SortColumn} {SortDir.ToString().ToLower()}");
        }
    }
}
