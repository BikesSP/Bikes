using ClientService.Application.Trips.Model;
using ClientService.Application.UserPost.Model;
using ClientService.Domain.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientService.Application.Trips.Query
{
    public class GetTripDetailRequest : IRequest<Response<TripDetailResponse>>
    {
        public long Id { get; set; }
    }
}
