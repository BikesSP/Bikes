using ClientService.Application.Common.Models.Response;
using ClientService.Domain.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientService.Application.UserTrip.Command
{
    public class StartTripRequest : IRequest<Response<StatusResponse>>
    {
        public long Id { get; set; }
    }
}
