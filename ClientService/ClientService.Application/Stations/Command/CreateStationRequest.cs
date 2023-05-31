using ClientService.Application.Stations.Model;
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
        }
    }

    public class CreateStationRequest : IRequest<StationDetailResponse>
    {
        public string Name { get; set; }
    }
}
