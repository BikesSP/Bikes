using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using ClientService.Application.Common.Enums;
using ClientService.Application.Common.Exceptions;
using ClientService.Domain.Entities;
using ClientService.Infrastructure.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using static Amazon.S3.Util.S3EventNotification;

namespace ClientService.Application.Services.CurrentUserService;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _accessor;
    private readonly IConfiguration _configuration;
    private readonly IUnitOfWork _unitOfWork;
    public CurrentUserService(IHttpContextAccessor accessor, IConfiguration configuration, IUnitOfWork unitOfWork)
    {
        _accessor = accessor;
        _configuration = configuration;
        _unitOfWork = unitOfWork;
    }

    // Get current login email
    public string? CurrentPrincipal
    {
        get
        {
            var identity = _accessor?.HttpContext?.User.Identity as ClaimsIdentity;
            if (identity == null || !identity.IsAuthenticated) return null;

            var claims = identity.Claims;

            var email = claims.FirstOrDefault(o => o.Type == ClaimTypes.NameIdentifier)?.Value ?? null;
            return email;
        }
    }

    public async Task<Account> GetCurrentAccount(Func<IQueryable<Account>, IQueryable<Account>>? includeFunc = null)
    {
        var currentPrincipal = CurrentPrincipal;
        if (currentPrincipal == null)
        {
            throw new ApiException(ResponseCode.Unauthorized);
        }
        var accountQuery =
            await _unitOfWork.AccountRepository.GetAsync(
                expression: acc => acc.Email.ToLower().Equals(currentPrincipal.ToLower()),
                includeFunc: includeFunc,
                disableTracking: false);

        var account = accountQuery.FirstOrDefault();

        return account ?? throw new ApiException(ResponseCode.Unauthorized);
    }


    public ClaimsPrincipal GetCurrentPrincipalFromToken(string token)
    {
        var tokenValidationParams = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = _configuration["Jwt:Issuer"],
            ValidAudience = _configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ??
                                                                               throw new ArgumentException(
                                                                                   "Jwt:Key is required")))
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParams, out var securityToken);
        var jwtSecurityToken = securityToken as JwtSecurityToken;
        if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase))
        {
            throw new ApiException(ResponseCode.AuthErrorInvalidRefreshToken);
        }

        return principal;
    }
}

