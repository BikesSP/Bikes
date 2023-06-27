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
    public class UpdateStationStatusRequestValidator : AbstractValidator<UpdateStationStatusRequest>
    {
        public UpdateStationStatusRequestValidator()
        {
            RuleFor(model => model.Status).NotEmpty();
        }
    }
    public class UpdateStationStatusRequest : IRequest<Response<Boolean>>
    {
        public int Id { get; set; }
        public string Status { get; set; }
    }
}
