using ClientService.Application.Common.Interfaces;
using Google.Apis.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ClientService.Infrastructure.Services
{
    public class GoogleAuthService : IGoogleAuthService
    {
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly ILogger<GoogleAuthService> _logger;

        public GoogleAuthService(IConfiguration configuration, ILogger<GoogleAuthService> logger)
        {
            _logger = logger;
            _clientId = configuration["Google:ClientId"] ?? throw new ArgumentException("Google:ClientId is required");
            _clientSecret = configuration["Google:ClientSecret"] ?? throw new ArgumentException("Google:ClientSecret is required");
        }

        public async Task<GoogleJsonWebSignature.Payload?> VerifyGoogleIdToken(string idToken)
        {
            try
            {
                var settings = new GoogleJsonWebSignature.ValidationSettings
                {
                    Audience = new List<string>
                {
                    _clientId
                }
                };

                var payload = await GoogleJsonWebSignature.ValidateAsync(idToken, settings);
                return payload;
            }
            catch (Exception ex)
            {
                _logger.LogError("Validate ID Token failed. {0}", ex.Message);
                return null;
            }
        }
    }
}
