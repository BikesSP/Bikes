using ClientService.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientService.Domain.Entities
{
    public class Vehicle : BaseAuditableEntity
    {
        public string? Brand { get; set; }
        public string? LicensePlate { get; set; }
        public string? Color { get; set; }
        public string? ImageUrl { get; set; }
        public string? Description { get; set; }
        public VehicleStatus Status { get; set; }
        public VehicleType Type { get; set; }
    }
}
