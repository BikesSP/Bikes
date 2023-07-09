using ClientService.Application.Common.Models.Response;
using ClientService.Domain.Wrappers;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientService.Application.ExponentPushToken.Commands
{

    public class AddExpoTokenValidator: AbstractValidator<AddExpoTokenRequest>
    {
        public AddExpoTokenValidator() {
            RuleFor(x => x.Token).NotEmpty();
        }
    }
    
    public class AddExpoTokenRequest: IRequest<Response<BaseBoolResponse>>
    {
        public string Token { get; set; }
    }
}
