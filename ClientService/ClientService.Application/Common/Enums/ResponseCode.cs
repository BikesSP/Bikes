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
        [Description("Unauthorized")] Unauthorized = 3,

        //Auth
        [Description("Google Id Token is invalid")]AuthErrorInvalidGoogleIdToken = 21,
        [Description("Refresh Token is invalid")] AuthErrorInvalidRefreshToken = 22,

        //Station
        [Description("Review not found")] StationErrorNotFound = 11,
    }
}
