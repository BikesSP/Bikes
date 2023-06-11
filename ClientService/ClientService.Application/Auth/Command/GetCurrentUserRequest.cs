using Amazon.Runtime.Internal;
using ClientService.Application.Auth.Model;
using ClientService.Domain.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientService.Application.Auth.Command
{
    public class GetCurrentUserRequest: IRequest<Response<UserProfileResponse?>>
    {
        public string Email { get; set; }
    }
}
