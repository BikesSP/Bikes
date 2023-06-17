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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientService.Application.UserPost.Handler
{
    public class RejectApplicationHandler: IRequestHandler<RejectApplicationRequest, Response<bool>>
    {
        private readonly ILogger<RejectApplicationHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public RejectApplicationHandler(
            ILogger<RejectApplicationHandler> logger, IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<Response<bool>> Handle(RejectApplicationRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var postQuery = await _unitOfWork.PostRepository.GetAsync(expression: x => x.Id == request.PostId, includeFunc: x => x.Include(post => post.Applier).Include(post => post.Author));
                var post = postQuery.FirstOrDefault();

                if (post?.Status != PostStatus.Created)
                {
                    return new Response<bool>(code: (int)ResponseCode.PostErrorNotFound, message: ResponseCode.PostErrorNotFound.GetDescription());
                }

                var user = await _currentUserService.GetCurrentAccount();
                if (post.AuthorId == user.Id)
                {
                    return new Response<bool>(code: (int)ResponseCode.Failed, message: ResponseCode.Failed.GetDescription());
                }

                var rejectedApplierQuery = await _unitOfWork.AccountRepository.GetAsync(x => x.Id.ToString() == request.ApplierId);
                var rejectedApplier = rejectedApplierQuery.FirstOrDefault();
                if (post.Applier == null)
                {
                    post.Applier = new List<Account>();
                }

                if (rejectedApplier == null || post.Applier.All(x => x.Id.ToString() != request.ApplierId))
                {
                    return new Response<bool>(code: (int)ResponseCode.PostErrorNotExistApplier, message: ResponseCode.PostErrorNotExistApplier.GetDescription());
                }

                post.Applier = post.Applier.FindAll(x => x.Id.ToString() != request.ApplierId);
                await _unitOfWork.PostRepository.UpdateAsync(post);
                var res = await _unitOfWork.SaveChangesAsync();

                //TODO: notify to reject applier
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
