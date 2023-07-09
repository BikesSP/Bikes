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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientService.Application.UserPost.Handler
{
    public class AcceptApplicationHandler: IRequestHandler<AcceptApplicationRequest, Response<BaseBoolResponse>>
    {
        private readonly ILogger<AcceptApplicationHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IExpoService _expoService;

        public AcceptApplicationHandler(
            ILogger<AcceptApplicationHandler> logger, IUnitOfWork unitOfWork, ICurrentUserService currentUserService, IExpoService expoService)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _expoService = expoService;
        }

        public async Task<Response<BaseBoolResponse>> Handle(AcceptApplicationRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var postQuery = await _unitOfWork.PostRepository.GetAsync(expression: x => x.Id == request.PostId, includeFunc: x => x.Include(post => post.Applier).Include(post => post.Author), disableTracking: false);
                var post = postQuery.FirstOrDefault();

                if (post?.Status != PostStatus.Created)
                {
                    return new Response<BaseBoolResponse>(code: (int)ResponseCode.PostErrorNotFound, message: ResponseCode.PostErrorNotFound.GetDescription());
                }

                var user = await _currentUserService.GetCurrentAccount();
                if(post.AuthorId != user.Id)
                {
                    return new Response<BaseBoolResponse>(code: (int)ResponseCode.Failed, message: ResponseCode.Failed.GetDescription());
                }

                var acceptedApplierQuery = await _unitOfWork.AccountRepository.GetAsync(x => x.Id.ToString() == request.ApplierId, includeFunc: query => query.Include(x => x.ExponentPushToken));
                var acceptedApplier = acceptedApplierQuery.FirstOrDefault();
                if (post.Applier == null)
                {
                    post.Applier = new List<Account>();
                }

                if(acceptedApplier == null || post.Applier.All(x => x.Id.ToString() != request.ApplierId))
                {
                    return new Response<BaseBoolResponse>(code: (int)ResponseCode.PostErrorNotExistApplier, message: ResponseCode.PostErrorNotExistApplier.GetDescription());
                }

                Account grabber;
                Account passenger;

                if(post.TripRole == Role.Grabber)
                {
                    grabber = post.Author;
                    passenger = acceptedApplier;
                } else
                {
                    passenger = post.Author;
                    grabber = acceptedApplier;
                }

                Trip trip = new Trip()
                {
                    GrabberId = grabber.Id,
                    PassengerId = passenger.Id,
                    TripStatus = TripStatus.Created,
                    StartAt = post.StartTime,
                    Description = post.Description,
                    Post = post,
                    StartStationId = post.StartStationId,
                    EndStationId = post.EndStationId,
                };

                await _unitOfWork.TripRepository.AddAsync(trip);


                //TODO: schedule reminder to remind coming trip?

                _expoService.sendTo(post.Author?.ExponentPushToken?.Token, new Services.ExpoService.Notification()
                {
                    Title = NotificationConstant.Title.POST_NEW_APPLICATION,
                    Body = String.Format(NotificationConstant.Body.POST_NEW_APPLICATION, user.Id),
                    Action = NotificationAction.OpenPost,
                    ReferenceId = post.Id.ToString(),
                });

                post.Status = PostStatus.Completed;
                await _unitOfWork.PostRepository.UpdateAsync(post);

                var res = await _unitOfWork.SaveChangesAsync();

                if(res > 0)
                    _expoService.sendTo(acceptedApplier?.ExponentPushToken?.Token, new Services.ExpoService.Notification()
                    {
                        Title = NotificationConstant.Title.POST_ACCEPT_APPLICATION,
                        Body = String.Format(NotificationConstant.Body.POST_ACCEPT_APPLICATION, post.AuthorId),
                        Action = NotificationAction.OpenTrip,
                        ReferenceId = post.Id.ToString(),
                    });

                return res > 0 ? new Response<BaseBoolResponse>(code: 0, data: new BaseBoolResponse() { Success=true }) : new Response<BaseBoolResponse>(code: (int)ResponseCode.Failed, message: ResponseCode.Failed.GetDescription());
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
