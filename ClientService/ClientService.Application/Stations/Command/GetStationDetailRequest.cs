using ClientService.Application.Stations.Model;
using ClientService.Domain.Wrappers;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientService.Application.Stations.Command
{
      public class GetStationDetailRequestValidator: AbstractValidator<GetStationDetailRequest>
    {
        public GetStationDetailRequestValidator()
        {
            RuleFor(model => model.id).NotEmpty();
        }
    }

    public class GetStationDetailRequest : IRequest<Response<StationDetailResponse>>
    {
        public int id;
    }
}
