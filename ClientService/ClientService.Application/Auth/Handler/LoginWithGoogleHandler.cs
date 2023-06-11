using ClientService.Application.Auth.Command;
using ClientService.Application.Auth.Model;
using ClientService.Application.Common.Enums;
using ClientService.Application.Services.GoogleAuthService;
using ClientService.Application.Services.JwtService;
using ClientService.Domain.Common;
using ClientService.Domain.Entities;
using ClientService.Domain.Wrappers;
using ClientService.Infrastructure.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ClientService.Application.Auth.Handler
{
    public class LoginWithGoogleHandler : IRequestHandler<LoginWithGoogleRequest, Response<TokenResponse?>>
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

        public async Task<Response<TokenResponse?>> Handle(LoginWithGoogleRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var payload = await _googleAuthService.VerifyGoogleIdToken(request.IdToken);
                if (payload == null || payload.Email == null)
                {
                    return new Response<TokenResponse?>(code: (int)ResponseCode.AuthErrorInvalidGoogleIdToken, message: "Invalid id token");
                }

                // Check if user existed
                var account = _unitOfWork.AccountRepository.FirstOrDefault(acc => acc.Email.ToLower().Equals(payload.Email.ToLower()));

                if (account == null)
                {
                    account = new Account()
                    {
                        Email = payload.Email,
                        Name = payload.Name ?? payload.Email,
                        Role = Role.User,
                        Phone = "",
                        AvartarUlr = payload.Picture
                    };
                    _unitOfWork.AccountRepository.Add(account);
                }

                // Generate jwt token 
                var accessToken = _jwtService.GenerateJwtToken(account);
                var refreshToken = _jwtService.GenerateJwtRefreshToken(account);

                // Save changes
                await _unitOfWork.SaveChangesAsync();

                return new Response<TokenResponse?>(code: 0, data: new TokenResponse(accessToken, refreshToken));
            }
            catch (Exception ex)
            {
                return new Response<TokenResponse?>(code: -1, message: "Internal server error");
            }
            finally
            {
            }
        }
    }
}
