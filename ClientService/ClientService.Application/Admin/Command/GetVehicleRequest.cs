using ClientService.Application.Stations.Model;
using ClientService.Application.User.Model;
using ClientService.Domain.Wrappers;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientService.Application.Vehicles.Command
{
    public class GetVehicleRequestValidator : AbstractValidator<GetVehicleRequest>
    {
        public GetVehicleRequestValidator()
        {
            RuleFor(model => model.id).NotEmpty();
        }
    }

    public class GetVehicleRequest : IRequest<Response<VehicleResponse>>
    {
        public string id;
    }
}
