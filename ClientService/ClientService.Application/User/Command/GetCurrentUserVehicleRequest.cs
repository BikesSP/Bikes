using ClientService.Application.User.Model;
using ClientService.Domain.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientService.Application.User.Command
{
    public class GetCurrentUserVehicleRequest: IRequest<Response<VehicleResponse?>>
    {
    }
}
