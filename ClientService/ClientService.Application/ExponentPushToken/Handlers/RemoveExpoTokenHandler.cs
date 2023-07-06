using ClientService.Application.Common.Enums;
using ClientService.Application.Common.Extensions;
using ClientService.Application.Common.Models.Response;
using ClientService.Application.ExponentPushToken.Commands;
using ClientService.Application.Services.CurrentUserService;
using ClientService.Application.Stations.Model;
using ClientService.Application.Trips.Model;
using ClientService.Application.UserPost.Handler;
using ClientService.Domain.Common;
using ClientService.Domain.Entities;
using ClientService.Domain.Wrappers;
using ClientService.Infrastructure.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientService.Application.ExponentPushToken.Handlers
{
    public class RemoveExpoTokenHandler : IRequestHandler<RemoveExpoTokenRequest, Response<BaseBoolResponse>>
    {
        private readonly ILogger<RemoveExpoTokenHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public RemoveExpoTokenHandler(
            ILogger<RemoveExpoTokenHandler> logger, IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<Response<BaseBoolResponse>> Handle(RemoveExpoTokenRequest request, CancellationToken cancellationToken)
        {
            var user = await _currentUserService.GetCurrentAccount();

            if(user == null)
            {
                return new Response<BaseBoolResponse>(code: (int)ResponseCode.AccountErrorNotFound, message: ResponseCode.AccountErrorNotFound.GetDescription());
            }

            var res = await _unitOfWork.ExponentPushTokenRepostiory.GetAsync(x => x.Token == request.Token && x.AccountId == user.Id);
            var tokenEntity = res.FirstOrDefault();
            await _unitOfWork.ExponentPushTokenRepostiory.DeleteAsync(tokenEntity);
            var result = await _unitOfWork.SaveChangesAsync();

            return new Response<BaseBoolResponse>(code: 0, data: new BaseBoolResponse()
                {
                    Success= result > 0
            });
        }
    }
}
