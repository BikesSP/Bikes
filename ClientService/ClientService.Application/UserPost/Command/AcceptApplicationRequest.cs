using ClientService.Application.Common.Models.Response;
using ClientService.Domain.Wrappers;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientService.Application.UserPost.Command
{
    public class AcceptApplicationValidator: AbstractValidator<AcceptApplicationRequest>
    {
        public AcceptApplicationValidator()
        {
            RuleFor(x => x.PostId).GreaterThan(0);
            RuleFor(x => x.ApplierId).NotNull().NotEmpty();
        }
    }
    public class AcceptApplicationRequest: IRequest<Response<BaseBoolResponse>>
    {
        public long PostId { get; set; }
        public string ApplierId { get; set; }
    }
}
