using ClientService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ClientService.Application.Common.Interfaces
{
    public interface ICurrentUserService
    {
        public string? CurrentPrincipal { get; }

        public ClaimsPrincipal GetCurrentPrincipalFromToken(string token);

        Task<Account> GetCurrentAccount(List<Expression<Func<Account, object>>> includes = null);

    }
}
