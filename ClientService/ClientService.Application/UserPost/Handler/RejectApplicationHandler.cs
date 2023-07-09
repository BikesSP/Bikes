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
    public class RejectApplicationHandler: IRequestHandler<RejectApplicationRequest, Response<BaseBoolResponse>>
    {
        private readonly ILogger<RejectApplicationHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IExpoService _expoService;

        public RejectApplicationHandler(
            ILogger<RejectApplicationHandler> logger, IUnitOfWork unitOfWork, ICurrentUserService currentUserService, IExpoService expoService)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _expoService = expoService;
        }

        public async Task<Response<BaseBoolResponse>> Handle(RejectApplicationRequest request, CancellationToken cancellationToken)
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
                if (post.AuthorId != user.Id)
                {
                    return new Response<BaseBoolResponse>(code: (int)ResponseCode.Failed, message: ResponseCode.Failed.GetDescription());
                }

                var rejectedApplierQuery = await _unitOfWork.AccountRepository.GetAsync(x => x.Id.ToString() == request.ApplierId);
                var rejectedApplier = rejectedApplierQuery.FirstOrDefault();
                if (post.Applier == null)
                {
                    post.Applier = new List<Account>();
                }

                if (rejectedApplier == null || post.Applier.All(x => x.Id.ToString() != request.ApplierId))
                {
                    return new Response<BaseBoolResponse>(code: (int)ResponseCode.PostErrorNotExistApplier, message: ResponseCode.PostErrorNotExistApplier.GetDescription());
                }

                post.Applier = post.Applier.FindAll(x => x.Id.ToString() != request.ApplierId);
                await _unitOfWork.PostRepository.UpdateAsync(post);
                var res = await _unitOfWork.SaveChangesAsync();

                if (res > 0)
                    _expoService.sendTo(rejectedApplier?.ExponentPushToken?.Token, new Services.ExpoService.Notification()
                    {
                        Title = NotificationConstant.Title.POST_REJECT_APPLICATION,
                        Body = String.Format(NotificationConstant.Body.POST_REJECT_APPLICATION, post.AuthorId),
                        Action = NotificationAction.OpenPost,
                        ReferenceId = post.Id.ToString(),
                    });

                //TODO: notify to reject applier
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
