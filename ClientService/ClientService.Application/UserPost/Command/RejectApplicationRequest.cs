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
    public class RejectApplicationValidator : AbstractValidator<RejectApplicationRequest>
    {
        public RejectApplicationValidator()
        {
            RuleFor(x => x.PostId).GreaterThan(0);
            RuleFor(x => x.ApplierId).NotNull().NotEmpty();
        }
    }
    public class RejectApplicationRequest : IRequest<Response<bool>>
    {
        public long PostId { get; set; }
        public string ApplierId { get; set; }
    }
}
