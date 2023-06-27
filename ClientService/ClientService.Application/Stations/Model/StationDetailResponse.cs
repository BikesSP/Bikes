using ClientService.Domain.Common;
using ClientService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientService.Application.Stations.Model
{
    public class StationDetailResponse
    {
        public float Id { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        public string Address { get; set; }

        public float Latitude { get; set; }

        public float Longitude { get; set; }

        public string Status { get; set; }

        public List<StationDetailResponse>? NextStations { get; set; }
    }
}
