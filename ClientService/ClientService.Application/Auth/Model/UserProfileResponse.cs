using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientService.Application.Auth.Model
{
    public class UserProfileResponse
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Avatar { get; set; }
        public int AveragePoint { get; set; }
        public bool IsUpdated { get; set; }
    }
}
