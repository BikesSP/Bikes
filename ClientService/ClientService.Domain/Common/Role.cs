using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientService.Domain.Common
{
    public enum Role
    {
        [Description("ADMIN")]
        Admin,
        [Description("USER")]
        User,
        [Description("GRABBER")]
        Grabber,
        [Description("PASSENGER")]
        Passenger
    }
}
