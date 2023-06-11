using ClientService.Domain.Entities;
using ClientService.Infrastructure.Persistence;
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
        public StationRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
