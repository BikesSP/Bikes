using ClientService.Application.User.Model;
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

namespace ClientService.Application.User.Command
{
    public class UpdateVehicleValidator : AbstractValidator<UpdateVehicleRequest>
    {
        public UpdateVehicleValidator() {

            RuleFor(x => x.Brand).NotEmpty();
            RuleFor(x => x.LicencePlate).NotEmpty();
            RuleFor(x => x.Color).NotEmpty();
            RuleFor(x => x.Image).NotEmpty();
            RuleFor(x => x.Description).NotEmpty();
            RuleFor(x => x.Type).NotEmpty();
        }
    }
    public class UpdateVehicleRequest: IRequest<Response<VehicleResponse?>>
    {
        public string Brand { get; set; }
        public string LicencePlate { get; set; }
        public string Color { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
    }
}
