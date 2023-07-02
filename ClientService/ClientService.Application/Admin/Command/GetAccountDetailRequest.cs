using ClientService.Application.User.Model;
using ClientService.Domain.Wrappers;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientService.Application.Admin.Command
{
    public class GetAccountDetailRequestValidator : AbstractValidator<GetAccountDetailRequest>
    {
        public GetAccountDetailRequestValidator()
        {
            RuleFor(model => model.Id).NotEmpty();
        }
    }

    public class GetAccountDetailRequest:  IRequest<Response<UserProfileResponse>>
    {
        public string Id { get; set; }
    }
}
