using ClientService.Application.Auth.Command;
using ClientService.Application.Auth.Model;
using ClientService.Application.Common.Enums;
using ClientService.Application.Common.Exceptions;
using ClientService.Application.Common.Extensions;
using ClientService.Application.Services.CurrentUserService;
using ClientService.Application.Services.JwtService;
using ClientService.Domain.Wrappers;
using ClientService.Infrastructure.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ClientService.Application.Auth.Handler
{
    public class RefreshTokenHandler : IRequestHandler<RefreshTokenRequest, Response<TokenResponse>>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IJwtService _jwtService;
        private readonly IUnitOfWork _unitOfWork;

        public RefreshTokenHandler(
            ICurrentUserService currentUserService,
            IJwtService jwtService,
            IUnitOfWork unitOfWork)
        {
            _currentUserService = currentUserService;
            _jwtService = jwtService;
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<TokenResponse>> Handle(RefreshTokenRequest request, CancellationToken cancellationToken)
        {
            var principal = _currentUserService.GetCurrentPrincipalFromToken(request.RefreshToken);
            var email = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (email == null)
            {
                return new Response<TokenResponse>(code: (int)ResponseCode.AuthErrorInvalidRefreshToken, message: ResponseCode.AuthErrorInvalidRefreshToken.GetDescription());
                
            }

            var accountQuery = await _unitOfWork.AccountRepository.GetAsync(
                account => account.Email.ToLower().Equals(email.ToLower())
            );

            var account = accountQuery.FirstOrDefault();
            if (account == null)
            {
                return new Response<TokenResponse>(code: (int)ResponseCode.AuthErrorInvalidRefreshToken, message: ResponseCode.AuthErrorInvalidRefreshToken.GetDescription());
                
            }

            var newAccessToken = _jwtService.GenerateJwtToken(account);
            var newRefreshToken = _jwtService.GenerateJwtRefreshToken(account);

            return new Response<TokenResponse>(code: 0, data: new TokenResponse(newAccessToken, newRefreshToken));

        }
    }
}
