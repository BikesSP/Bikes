using ClientService.Application.Common.Models.Response;
using ClientService.Domain.Wrappers;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ClientService.Application.User.Command
{
    public class UpdateVehicleStatusValidator: AbstractValidator<UpdateVehicleStatusRequest>
    {
        public UpdateVehicleStatusValidator() {
            RuleFor(x => x.Approved).NotEmpty();
        }
    }
    public class UpdateVehicleStatusRequest: IRequest<Response<BaseBoolResponse>>
    {
        [JsonIgnore]
        public string Id { get; set; }
        public bool Approved { get; set; }
    }
}
