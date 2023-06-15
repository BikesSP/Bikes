using ClientService.Domain.Entities;
using ClientService.Infrastructure.Persistence;
using Repository.Repositories.BaseRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientService.Infrastructure.Repositories.TripRepository
{
    public class TripRepository : BaseRepository<ApplicationDbContext, Trip>, ITripRepository
    {
        public TripRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
