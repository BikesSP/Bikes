using ClientService.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientService.Domain.Entities
{
    public class Station
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public float Longitude { get; set; }
        public float Latitude { get; set; }
        public ObjectStatus ObjectStatus { get; set; }
        public virtual List<Station>? NextStation { get; set; }
        public virtual List<Station>? PreviousStation { get; set; }
    }
}
