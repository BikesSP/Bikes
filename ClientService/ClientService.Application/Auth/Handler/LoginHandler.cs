using ClientService.Application.Auth.Command;
using ClientService.Application.Auth.Model;
using ClientService.Application.Common.Enums;
using ClientService.Application.Common.Exceptions;
using ClientService.Application.Services.JwtService;
using ClientService.Domain.Common;
using ClientService.Domain.Wrappers;
using ClientService.Infrastructure.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientService.Application.Auth.Handler
{
    public class LoginHandler : IRequestHandler<LoginRequest, Response<TokenResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtService _jwtService;
        private readonly ILogger<LoginHandler> _logger;

        public LoginHandler(IUnitOfWork unitOfWork, ILogger<LoginHandler> logger, IJwtService jwtService)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _jwtService = jwtService;
        }

        public async Task<Response<TokenResponse?>> Handle(LoginRequest request, CancellationToken cancellationToken)
        {
            var userQuery = await _unitOfWork.AccountRepository.GetAsync(
                    expression: account => account.Email.ToLower().Equals(request.Email.ToLower())
                                          && account.Password != null
                                          && account.Password.Equals(request.Password)
                );

            var user = userQuery.FirstOrDefault();
            if (user == null)
            {
                throw new ApiException(ResponseCode.InvalidUsernameOrPassword);
            }

            if (!user.Role.Equals(Role.Admin))
            {
                throw new ApiException(ResponseCode.InvalidUsernameOrPassword);
            }

            var token = _jwtService.GenerateJwtToken(user);
            var refreshToken = _jwtService.GenerateJwtRefreshToken(user);

            return new Response<TokenResponse?>(code: 0, data: new TokenResponse(token, refreshToken));
        }
    }

}
