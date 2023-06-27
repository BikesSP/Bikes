using ClientService.Application.Stations.Model;
using ClientService.Domain.Wrappers;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ClientService.Application.Stations.Command.CreateStationRequestValidator;

namespace ClientService.Application.Stations.Command
{
    public class CreateStationRequestValidator : AbstractValidator<CreateStationRequest>
    {
        public CreateStationRequestValidator() { 
            RuleFor(model => model.Name)
                .NotEmpty();
            RuleFor(model => model.Description).NotEmpty();
            RuleFor(model => model.Address).NotEmpty();
            RuleFor(model => model.Latitude).NotEmpty();
            RuleFor(model => model.Longitude).NotEmpty();
        }
    }

    public class CreateStationRequest : IRequest<Response<StationDetailResponse?>>
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string Address { get; set; } 

        public float Longitude { get; set; }
        public float Latitude { get; set; }

        public List<long>? NextStationsIds { get; set; }

    }
}
