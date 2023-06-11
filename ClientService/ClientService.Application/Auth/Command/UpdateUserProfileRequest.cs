using ClientService.Application.Auth.Model;
using ClientService.Domain.Wrappers;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ClientService.Application.Auth.Command
{

    public class UpdateUserProfileValidator: AbstractValidator<UpdateUserProfileRequest>
    {
        public UpdateUserProfileValidator() { 
            RuleFor(x => x.Avatar).NotEmpty();
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Phone).NotEmpty();
            RuleFor(x => x.Card).NotEmpty();
        }
    }
    public class UpdateUserProfileRequest: IRequest<Response<UserProfileResponse?>>
    {
        [JsonIgnore]
        public string Email { get; set; }
        public string Name { get; set;}
        public string Phone { get; set;}
        public string Avatar { get; set;}
        public string Card { get; set;}
    }
}
