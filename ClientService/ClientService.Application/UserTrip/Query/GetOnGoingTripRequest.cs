using ClientService.Application.UserTrip.Model;
using ClientService.Domain.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientService.Application.UserTrip.Query
{
    public class GetOnGoingTripRequest : IRequest<Response<UserTripDetailResponse>>
    {

    }
}
