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
        public long Id;
        public String Name;
        public String Address;
        public String Description;
        public float Longitude;
        public float Latitude;

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
