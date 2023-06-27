using ClientService.Application.Stations.Model;
using ClientService.Domain.Common;
using ClientService.Domain.Wrappers;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ClientService.Application.Stations.Command
{


    public class UpdateStationRequestValidator : AbstractValidator<UpdateStationRequest>
    {
        public UpdateStationRequestValidator()
        {
            RuleFor(model => model.Name)
              .NotEmpty();
            RuleFor(model => model.Description).NotEmpty();
            RuleFor(model => model.Address).NotEmpty();
        }
    }
    public class UpdateStationRequest : IRequest<Response<StationDetailResponse>>
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        public string Address { get; set; }

        public float? Longitude { get; set; }
        public float? Latitude { get; set; }

        public List<long>? NextStationsIds { get; set; }
    }
}
