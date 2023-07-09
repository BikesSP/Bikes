using ClientService.Application.Common.Constants;
using ClientService.Application.Common.Enums;
using ClientService.Application.Common.Extensions;
using ClientService.Application.Services.CurrentUserService;
using ClientService.Application.Services.ExpoService;
using ClientService.Application.UserPost.Command;
using ClientService.Application.UserPost.Model;
using ClientService.Domain.Common;
using ClientService.Domain.Common.Enums.Notification;
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
    public class CancelPostHandler: IRequestHandler<CancelPostRequest, Response<PostResponse?>>
    {
        private readonly ILogger<CancelPostHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IExpoService _expoService;

        public CancelPostHandler(
            ILogger<CancelPostHandler> logger, IUnitOfWork unitOfWork, ICurrentUserService currentUserService, IExpoService expoService)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _expoService = expoService;
        }

        public async Task<Response<PostResponse?>> Handle(CancelPostRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var postQuery = await _unitOfWork.PostRepository.GetAsync(expression: x => x.Id == request.Id, includeFunc: x => x.Include(post => post.Applier).ThenInclude(applier => applier.ExponentPushToken).Include(post => post.StartStation).Include(post => post.EndStation).Include(post => post.Author), disableTracking: false);
                var post = postQuery.FirstOrDefault();

                if (post?.Status != PostStatus.Created)
                {
                    return new Response<PostResponse?>(code: (int)ResponseCode.PostErrorNotFound, message: ResponseCode.PostErrorNotFound.GetDescription());
                }

                var user = await _currentUserService.GetCurrentAccount();
                if (post.AuthorId != user.Id)
                {
                    return new Response<PostResponse?>(code: (int)ResponseCode.Failed, message: ResponseCode.Failed.GetDescription());
                }

                post.Status = PostStatus.Completed;
                await _unitOfWork.PostRepository.UpdateAsync(post);

                var res = await _unitOfWork.SaveChangesAsync();

                if(res > 0)
                    _expoService.sendList(post.Applier.ConvertAll(x => x.ExponentPushToken.Token), new Services.ExpoService.Notification()
                    {
                        Title = NotificationConstant.Title.POST_ACCEPT_APPLICATION,
                        Body = String.Format(NotificationConstant.Body.POST_ACCEPT_APPLICATION, post.AuthorId),
                        Action = NotificationAction.OpenTrip,
                        ReferenceId = post.Id.ToString(),
                    });

                return res > 0 ?
                    new Response<PostResponse?>(code: 0, data: new PostResponse()
                    {
                        Id = post.Id,
                        Role = post.TripRole.GetDescription().ToUpper(),
                        Description = post.Description,
                        StartStationId = post.StartStationId,
                        EndStationId = post.EndStationId,
                        AuthorId = post.AuthorId,
                        AuthorName = user.Name,
                        Status = post.Status.ToString().ToUpper(),
                        StartTime = post.StartTime,
                        FeedbackContent = post.FeedbackContent,
                        FeedbackPoint = post.FeedbackPoint,
                        StartStation = post.StartStation.Name,
                        EndStation = post.EndStation.Name,
                        CreatedAt = post.CreatedAt,
                        UpdatedAt = post.UpdatedAt
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
