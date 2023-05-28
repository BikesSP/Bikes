using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientService.Application.Common.Enums
{
    public enum ResponseCode
    {
        [Description("Validation Error")] ErrorValidation = 1,
        [Description("Common Error")] ErrorCommon = 2,

        //Station
        [Description("Review not found")] StationErrorNotFound = 11,
    }
}
