using ClientService.Domain.Entities;
using ClientService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Repository.Repositories.BaseRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientService.Infrastructure.Repositories.ExponentPushTokenRepository
{
    public class ExponentPushTokenRepostiory : BaseRepository<ApplicationDbContext, ExponentPushToken>, IExponentPushTokenRepostiory
    {
        public ExponentPushTokenRepostiory(DbContext dbContext) : base(dbContext)
        {
        }
    }
}
