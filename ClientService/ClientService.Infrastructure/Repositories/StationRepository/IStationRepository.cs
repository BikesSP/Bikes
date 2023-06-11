using ClientService.Domain.Entities;
using Repository.Repositories.BaseRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientService.Infrastructure.Repositories.StationRepository
{
    public interface IStationRepository : IBaseRepository<Station>
    {
    }
}
