using Google.Apis.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientService.Application.Services.GoogleAuthService
{
    public interface IGoogleAuthService
    {
        public Task<GoogleJsonWebSignature.Payload?> VerifyGoogleIdToken(string idToken);
    }

}
