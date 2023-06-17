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
    public class CancelApplicationValidator: AbstractValidator<CancelApplicationRequest>
    {
        public CancelApplicationValidator() {
            RuleFor(x => x.PostId).GreaterThan(0);
        }
    }
    public class CancelApplicationRequest: IRequest<Response<bool>>
    {
        public long PostId { get; set; }
    }
}
