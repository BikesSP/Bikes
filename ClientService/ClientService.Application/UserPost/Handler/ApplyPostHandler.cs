using ClientService.Application.Common.Constants;
using ClientService.Application.Common.Enums;
using ClientService.Application.Common.Extensions;
using ClientService.Application.Common.Models.Response;
using ClientService.Application.Services.CurrentUserService;
using ClientService.Application.Services.ExpoService;
using ClientService.Application.UserPost.Command;
using ClientService.Domain.Common;
using ClientService.Domain.Common.Enums.Notification;
using ClientService.Domain.Entities;
using ClientService.Domain.Wrappers;
using ClientService.Infrastructure.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ClientService.Application.UserPost.Handler
{
    public class ApplyPostHandler: IRequestHandler<ApplyPostRequest, Response<BaseBoolResponse>>
    {
        private readonly ILogger<ApplyPostHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IExpoService _expoService;

        public ApplyPostHandler(
            ILogger<ApplyPostHandler> logger, IUnitOfWork unitOfWork, ICurrentUserService currentUserService, IExpoService expoService)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _expoService = expoService;
        }

        public async Task<Response<BaseBoolResponse>> Handle(ApplyPostRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _currentUserService.GetCurrentAccount();
                if (!user.IsUpdated)
                {
                    return new Response<BaseBoolResponse>(code: (int)ResponseCode.PostErrorUnupdatedAccount, message: ResponseCode.PostErrorUnupdatedAccount.GetDescription());
                }

                var postQuery = await _unitOfWork.PostRepository.GetAsync(expression: x => x.Id == request.Id, includeFunc: x => x.Include(post => post.Applier).Include(post => post.Author).ThenInclude(author => author.ExponentPushToken), disableTracking: false);
                var post = postQuery.FirstOrDefault();

                if (post?.Status != PostStatus.Created) { 
                    return new Response<BaseBoolResponse>(code: (int)ResponseCode.PostErrorNotFound, message: ResponseCode.PostErrorNotFound.GetDescription());
                }

                if (post.AuthorId == user.Id)
                {
                    return new Response<BaseBoolResponse>(code: (int)ResponseCode.PostErrorSelfApply, message: ResponseCode.PostErrorSelfApply.GetDescription());
                }

                if(post.TripRole == Role.Passenger && (user.LicensePlate == null || user.Status != VehicleStatus.Approved)) {
                    return new Response<BaseBoolResponse>(code: (int)ResponseCode.PostErrorUnregisteredVehicle, message: ResponseCode.PostErrorUnregisteredVehicle.GetDescription());
                }

                var tripQuery = await _unitOfWork.TripRepository.GetAsync(x => x.Post.Id == request.Id && x.TripStatus == TripStatus.OnGoing, includeFunc: x => x.Include(trip => trip.Post));
                var trip = tripQuery.FirstOrDefault();
                if(trip != null)
                {
                    return new Response<BaseBoolResponse>(code: (int)ResponseCode.TripErrorOngoingTrip, message: ResponseCode.TripErrorOngoingTrip.GetDescription());
                }

                //TODO: check existed post

                if(post.Applier == null)
                {
                    post.Applier = new List<Account>();
                }
                if(post.Applier.Any(x => x.Id == user.Id))
                {
                    return new Response<BaseBoolResponse>(code: (int)ResponseCode.PostErrorExistedApplier, message: ResponseCode.PostErrorExistedApplier.GetDescription());
                }

                post.Applier.Add(user);
                await _unitOfWork.PostRepository.UpdateAsync(post);
                var res = await _unitOfWork.SaveChangesAsync();

                if(res > 0)
                    _expoService.sendTo(post.Author.ExponentPushToken.Token, new Services.ExpoService.Notification()
                    {
                        Title = NotificationConstant.Title.POST_NEW_APPLICATION,
                        Body = String.Format(NotificationConstant.Body.POST_NEW_APPLICATION, user.Id),
                        Action = NotificationAction.OpenPost,
                        ReferenceId = post.Id.ToString(),
                    });

                return new Response<BaseBoolResponse>(code: 0, data: new BaseBoolResponse() { Success = res > 0 });

            }
            catch (Exception ex)
            {
                return new Response<BaseBoolResponse>(code: (int)ResponseCode.Failed, message: ResponseCode.Failed.GetDescription());
            }
            finally
            {
            }
        }
    }
}
