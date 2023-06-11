using ClientService.Domain.Entities;
using ClientService.Infrastructure.Persistence;
using Repository.Repositories.BaseRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientService.Infrastructure.Repositories.AccountRepository
{
    public class AccountRepository : BaseRepository<ApplicationDbContext, Account>, IAccountRepository
    {
        public AccountRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
