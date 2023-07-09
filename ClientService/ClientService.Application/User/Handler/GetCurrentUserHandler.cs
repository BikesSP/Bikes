using ClientService.Application.Common.Enums;
using ClientService.Application.Common.Extensions;
using ClientService.Application.Services.CurrentUserService;
using ClientService.Application.Services.GoogleAuthService;
using ClientService.Application.Services.JwtService;
using ClientService.Application.User.Command;
using ClientService.Application.User.Model;
using ClientService.Domain.Wrappers;
using ClientService.Infrastructure.Repositories;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientService.Application.User.Handler
{
    public class GetCurrentUserHandler : IRequestHandler<GetCurrentUserRequest, Response<UserProfileResponse?>>
    {

        private readonly ILogger<GetCurrentUserHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public GetCurrentUserHandler(
            ILogger<GetCurrentUserHandler> logger, IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }
        public async Task<Response<UserProfileResponse?>> Handle(GetCurrentUserRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _currentUserService.GetCurrentAccount();
                if (user == null)
                {
                    return new Response<UserProfileResponse?>(code: (int)ResponseCode.Failed, message: ResponseCode.Failed.GetDescription());
                }
                return new Response<UserProfileResponse?>(code: 0,
                    data: new UserProfileResponse()
                    {
                        Avatar = user.AvartarUlr,
                        AveragePoint = 0,
                        Email = user.Email,
                        Id = user.Id.ToString(),
                        IsUpdated = user.IsUpdated,
                        Name = user.Name,
                        Phone = user.Phone,
                        Card = user.Card
                    }
                    );
            }
            catch (Exception ex)
            {
                return new Response<UserProfileResponse?>(code: (int)ResponseCode.Failed, message: ResponseCode.Failed.GetDescription());
            }
            finally
            {
            }
        }
    }
}
