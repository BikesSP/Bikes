using ClientService.Application.Common.Enums;
using ClientService.Application.Common.Extensions;
using ClientService.Application.Services.CurrentUserService;
using ClientService.Application.UserPost.Command;
using ClientService.Domain.Common;
using ClientService.Domain.Entities;
using ClientService.Domain.Wrappers;
using ClientService.Infrastructure.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ClientService.Application.UserPost.Handler
{
    public class ApplyPostHandler: IRequestHandler<ApplyPostRequest, Response<bool>>
    {
        private readonly ILogger<ApplyPostHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public ApplyPostHandler(
            ILogger<ApplyPostHandler> logger, IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<Response<bool>> Handle(ApplyPostRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _currentUserService.GetCurrentAccount();
                if (!user.IsUpdated)
                {
                    return new Response<bool>(code: (int)ResponseCode.PostErrorUnupdatedAccount, message: ResponseCode.PostErrorUnupdatedAccount.GetDescription());
                }

                var postQuery = await _unitOfWork.PostRepository.GetAsync(expression: x => x.Id == request.Id, includeFunc: x => x.Include(post => post.Applier), disableTracking: false);
                var post = postQuery.FirstOrDefault();

                if (post?.Status != PostStatus.Created) { 
                    return new Response<bool>(code: (int)ResponseCode.PostErrorNotFound, message: ResponseCode.PostErrorNotFound.GetDescription());
                }

                if (post.AuthorId == user.Id)
                {
                    return new Response<bool>(code: (int)ResponseCode.PostErrorSelfApply, message: ResponseCode.PostErrorSelfApply.GetDescription());
                }

                if(post.TripRole == Role.Passenger && (user.LicensePlate == null || user.Status != VehicleStatus.Approved)) {
                    return new Response<bool>(code: (int)ResponseCode.PostErrorUnregisteredVehicle, message: ResponseCode.PostErrorUnregisteredVehicle.GetDescription());
                }

                var tripQuery = await _unitOfWork.TripRepository.GetAsync(x => x.Post.Id == request.Id && x.TripStatus == TripStatus.OnGoing, includeFunc: x => x.Include(trip => trip.Post));
                var trip = tripQuery.FirstOrDefault();
                if(trip != null)
                {
                    return new Response<bool>(code: (int)ResponseCode.TripErrorOngoingTrip, message: ResponseCode.TripErrorOngoingTrip.GetDescription());
                }

                //TODO: check existed post

                if(post.Applier == null)
                {
                    post.Applier = new List<Account>();
                }
                if(post.Applier.Any(x => x.Id == user.Id))
                {
                    return new Response<bool>(code: (int)ResponseCode.PostErrorExistedApplier, message: ResponseCode.PostErrorExistedApplier.GetDescription());
                }

                post.Applier.Add(user);
                await _unitOfWork.PostRepository.UpdateAsync(post);
                var res = await _unitOfWork.SaveChangesAsync();

                //TODO: push notification?

                return new Response<bool>(code: 0, data: res > 0);

            }
            catch (Exception ex)
            {
                return new Response<bool>(code: (int)ResponseCode.Failed, message: ResponseCode.Failed.GetDescription());
            }
            finally
            {
            }
        }
    }
}
