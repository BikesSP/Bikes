using ClientService.Application.Common.Enums;
using ClientService.Application.Common.Extensions;
using ClientService.Application.Services.CurrentUserService;
using ClientService.Application.User.Handler;
using ClientService.Application.User.Model;
using ClientService.Application.UserPost.Command;
using ClientService.Application.UserPost.Model;
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
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace ClientService.Application.UserPost.Handler
{
    public class CreatePostHandler: IRequestHandler<CreatePostRequest, Response<PostResponse?>>
    {
        private readonly ILogger<CreatePostHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public CreatePostHandler(
            ILogger<CreatePostHandler> logger, IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<Response<PostResponse?>> Handle(CreatePostRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _currentUserService.GetCurrentAccount();
                if(!user.IsUpdated)
                {
                    return new Response<PostResponse?>(code: (int)ResponseCode.PostErrorUnupdatedAccount, message: ResponseCode.PostErrorUnupdatedAccount.GetDescription());
                }

                var inProgressTripQuery = await _unitOfWork.TripRepository.GetAsync(x => (x.GrabberId == user.Id || x.PassengerId == user.Id) && x.TripStatus == Domain.Common.TripStatus.OnGoing);
                var inProgressTrip = inProgressTripQuery.FirstOrDefault();
                if (inProgressTrip != null)
                {
                    return new Response<PostResponse?>(code: (int)ResponseCode.TripErrorOngoingTrip, message: ResponseCode.TripErrorOngoingTrip.GetDescription());
                }

                if(user.LicensePlate == null && request.Role == Role.Grabber.GetDescription())
                {
                    return new Response<PostResponse?>(code: (int)ResponseCode.PostErrorUnregisteredVehicle, message: ResponseCode.PostErrorUnregisteredVehicle.GetDescription());
                }

                //TODO: fix later
                /*var existedPostQuery = await _unitOfWork.PostRepository.GetAsync(x => x.AuthorId == user.Id && Math.Abs((x.StartTime -request.StartTime).Minutes) <= 30);
                var existedPost = existedPostQuery.FirstOrDefault();
                if (existedPost != null)
                {
                    return new Response<PostResponse?>(code: (int)ResponseCode.PostErrorConflictTime, message: ResponseCode.PostErrorConflictTime.GetDescription());

                }*/


                if (request.EndStationId == request.StartStationId)
                {
                    return new Response<PostResponse?>(code: (int)ResponseCode.PostErrorInvalidStation, message: ResponseCode.PostErrorInvalidStation.GetDescription());
                }

                var stationsQuery = await _unitOfWork.StationRepository.GetAllAsync(expression: x => x.Id == request.EndStationId || x.Id == request.StartStationId, includeFunc: x => x.Include(station => station.NextStation));
                var stations = stationsQuery.ToList();
                if(stations.Count() != 2)
                {
                    return new Response<PostResponse?>(code: (int)ResponseCode.PostErrorStationNotFound, message: ResponseCode.PostErrorStationNotFound.GetDescription());
                }

                var startStation = stations.FirstOrDefault(x => x.Id == request.StartStationId);
                var endStation = stations.FirstOrDefault(x => x.Id == request.EndStationId);

                var isAllowedEndStation = startStation.NextStation.Any(x => x.Id == request.EndStationId);
                if(!isAllowedEndStation)
                {
                    return new Response<PostResponse?>(code: (int)ResponseCode.PostErrorInvalidEndStation, message: ResponseCode.PostErrorInvalidEndStation.GetDescription());
                }

                Post post = new Post()
                {
                    TripRole=request.Role == Role.Grabber.GetDescription().ToUpper() ? Role.Grabber : Role.Passenger,
                    Description=request.Description.Trim(),
                    StartStationId=request.StartStationId,
                    EndStationId=request.EndStationId,
                    AuthorId=user.Id,
                    StartTime=request.StartTime,
                    Status=PostStatus.Created,
                    FeedbackContent=""
                };

                await _unitOfWork.PostRepository.AddAsync(post);
                var result = await _unitOfWork.SaveChangesAsync();

                return result > 0 ? 
                    new Response<PostResponse?>(code: 0, data: new PostResponse()
                    {
                        Id=post.Id,
                        Role = post.TripRole.GetDescription().ToUpper(),
                        Description = post.Description,
                        StartStationId = post.StartStationId,
                        EndStationId = post.EndStationId,
                        AuthorId = post.AuthorId,
                        AuthorName = user.Name,
                        Status = post.Status.ToString().ToUpper(),
                        StartTime = post.StartTime,
                        StartStation=startStation.Name,
                        EndStation=endStation.Name,
                        CreatedAt=post.CreatedAt,
                        UpdatedAt=post.UpdatedAt
                    })
                    : new Response<PostResponse?>(code: (int)ResponseCode.Failed, message: ResponseCode.Failed.GetDescription());
            }
            catch (Exception ex)
            {
                return new Response<PostResponse?>(code: (int)ResponseCode.Failed, message: ResponseCode.Failed.GetDescription());
            }
            finally
            {
            }
        }
    }
}
