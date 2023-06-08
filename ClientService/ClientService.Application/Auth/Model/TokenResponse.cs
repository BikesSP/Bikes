using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientService.Application.Auth.Model
{
    public class TokenResponse
    {
        public string AccessToken { get; private set; }

        public string RefreshToken { get; private set; }


        public TokenResponse(string accessToken, string refreshToken)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        }
    }


}
