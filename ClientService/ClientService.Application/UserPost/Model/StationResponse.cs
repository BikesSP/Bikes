using ClientService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ClientService.Application.UserPost.Model
{
    public class StationResponse
    {
        public long Id { get; set; }
        public String Name { get; set; }
        public String Address { get; set; }
        public String Description { get; set; }
        public float Longitude { get; set; }
        public float Latitude { get; set; }

        public StationResponse()
        {

        }

        public StationResponse(Station station)
        {
            this.Id = station.Id;
            this.Name = station.Name;
            this.Address = station.Address;
            this.Description = station.Description;
            this.Longitude = station.Longitude;
            this.Latitude = station.Latitude;
        }
    }
}
