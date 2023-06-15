using ClientService.Application.UserPost.Model;
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
    public class CreatePostValidator: AbstractValidator<CreatePostRequest>
    {
        public CreatePostValidator() {
            RuleFor(x => x.StartStationId).NotNull();
            RuleFor(x => x.EndStationId).NotNull();
            RuleFor(x => x.StartTime).NotNull().Must(x => x > DateTimeOffset.UtcNow);
            RuleFor(x => x.Description).NotEmpty().MaximumLength(1000);
            RuleFor(x => x.Role).NotNull().NotEmpty();
        }
    }

    public class CreatePostRequest: IRequest<Response<PostResponse?>>
    {
        public long StartStationId { get; set; }
        public long EndStationId { get; set;}
        public DateTimeOffset StartTime { get; set; }
        public string Description { get; set; }
        public string Role { get; set; }
    }
}
