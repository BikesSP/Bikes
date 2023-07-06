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
    public class AddExpoTokenHandler: IRequestHandler<AddExpoTokenRequest, Response<BaseBoolResponse>>
    {
        private readonly ILogger<AddExpoTokenHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public AddExpoTokenHandler(
            ILogger<AddExpoTokenHandler> logger, IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<Response<BaseBoolResponse>> Handle(AddExpoTokenRequest request, CancellationToken cancellationToken)
        {
            var user = await _currentUserService.GetCurrentAccount();

            if(user == null)
            {
                return new Response<BaseBoolResponse>(code: (int)ResponseCode.AccountErrorNotFound, message: ResponseCode.AccountErrorNotFound.GetDescription());
            }

            await _unitOfWork.ExponentPushTokenRepostiory.AddAsync(new Domain.Entities.ExponentPushToken()
            {
                AccountId = user.Id,
                Token = request.Token,
            });
            var res = await _unitOfWork.SaveChangesAsync();

            return new Response<BaseBoolResponse>(code: 0, data: new BaseBoolResponse()
                {
                    Success= res > 0
                });
        }
    }
}
