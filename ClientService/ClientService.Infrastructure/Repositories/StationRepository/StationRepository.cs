using ClientService.Domain.Entities;
using ClientService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Repository.Repositories.BaseRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientService.Infrastructure.Repositories.StationRepository
{
    public class StationRepository : BaseRepository<ApplicationDbContext, Station>, IStationRepository
    {
        private readonly DbContext _dbContext;

        public StationRepository(ApplicationDbContext context) : base(context)
        {
            _dbContext = context ?? throw new ArgumentNullException(nameof(context));
        }
    }
}
