using ClientService.Application.Auth.Command;
using ClientService.Application.Auth.Model;
using ClientService.Application.Common.Enums;
using ClientService.Application.Common.Exceptions;
using ClientService.Application.Common.Interfaces;
using ClientService.Domain.Common;
using ClientService.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ClientService.Application.Auth.Handler
{
    public class LoginWithGoogleHandler : IRequestHandler<LoginWithGoogleRequest, TokenResponse>
    {
        private readonly IGoogleAuthService _googleAuthService;
        private readonly IJwtService _jwtService;
        private readonly ILogger<LoginWithGoogleHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public LoginWithGoogleHandler(
            IJwtService jwtService,
            IGoogleAuthService googleAuthService,
            ILogger<LoginWithGoogleHandler> logger, IUnitOfWork unitOfWork)
        {
            _jwtService = jwtService;
            _googleAuthService = googleAuthService;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<TokenResponse> Handle(LoginWithGoogleRequest request, CancellationToken cancellationToken)
        {
            var payload = await _googleAuthService.VerifyGoogleIdToken(request.IdToken);
            if (payload == null || payload.Email == null)
            {
                throw new ApiException(ResponseCode.AuthErrorInvalidGoogleIdToken);
            }

            // Check if user existed
            var accountQuery = await _unitOfWork.AccountRepository.GetAsync(acc => acc.Email.ToLower().Equals(payload.Email.ToLower()));

            var account = accountQuery.FirstOrDefault();
            if (account == null)
            {
                account = new Account()
                {
                    Email = payload.Email,
                    Name = payload.Name ?? payload.Email,
                    Role = Role.User,
                    AvartarUlr = payload.Picture
                };
                await _unitOfWork.AccountRepository.AddAsync(account);
            }

            // Generate jwt token 
            var accessToken = _jwtService.GenerateJwtToken(account);
            var refreshToken = _jwtService.GenerateJwtRefreshToken(account);

            // Save changes
            await _unitOfWork.SaveChangesAsync();

            return new TokenResponse(accessToken, refreshToken);
        }
    }
}
