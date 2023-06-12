using ClientService.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientService.Application.User.Model
{
    public class VehicleResponse
    {
        public int Id { get; set; }
        public string Brand { get; set; }
        public string LicencePlate { get; set; }
        public string Color { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public VehicleType Type { get; set; } 
        public VehicleStatus Status { get; set; }
    }
}
