using ClientService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace ClientService.Application.Common.Interfaces
{
    public interface IJwtService
    {
        string GenerateJwtToken(Account account);

        string GenerateJwtRefreshToken(Account account);

    }

}
