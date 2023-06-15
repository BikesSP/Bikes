using ClientService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static Amazon.S3.Util.S3EventNotification;

namespace ClientService.Application.Services.CurrentUserService
{
    public interface ICurrentUserService
    {
        public string? CurrentPrincipal { get; }

        public ClaimsPrincipal GetCurrentPrincipalFromToken(string token);
        Task<Account> GetCurrentAccount(Func<IQueryable<Account>, IQueryable<Account>>? includeFunc = null);

    }
}
