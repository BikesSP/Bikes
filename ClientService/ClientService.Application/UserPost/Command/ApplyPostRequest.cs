using ClientService.Domain.Wrappers;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ClientService.Application.UserPost.Command
{
    public class ApplyPostValidator: AbstractValidator<ApplyPostRequest>
    {
        public ApplyPostValidator() {
            RuleFor(x => x.Id).GreaterThan(0);
        }
    }
    public class ApplyPostRequest: IRequest<Response<bool>>
    {
        [JsonIgnore]
        public long Id { get; set; }
    }
}
