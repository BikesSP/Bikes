using ClientService.Domain.Entities;
using ClientService.Infrastructure.Persistence;
using Repository.Repositories.BaseRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientService.Infrastructure.Repositories.VehicleRepository
{
    public class VehicleRepository : BaseRepository<ApplicationDbContext, Vehicle>, IVehicleRepository
    {
        public VehicleRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
