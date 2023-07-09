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
    public class UpdateAccountStatusRequestValidator: AbstractValidator<UpdateAccountStatusRequest>
    {
        public UpdateAccountStatusRequestValidator()
        {
            RuleFor(model => model.Status).NotEmpty();
        }
    }

    public class UpdateAccountStatusRequest : IRequest<Response<Boolean>>
    {
        public string Id { get; set; }
        public string Status { get; set; }
    }
}
