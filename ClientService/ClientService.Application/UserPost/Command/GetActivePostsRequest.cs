using ClientService.Application.Common.Extensions;
using ClientService.Application.Common.Models.Request;
using ClientService.Application.UserPost.Model;
using ClientService.Domain.Common;
using ClientService.Domain.Entities;
using ClientService.Domain.Wrappers;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using System.Text.Json.Serialization;

namespace ClientService.Application.UserPost.Command
{
    public class GetActivePostValidtor: AbstractValidator<GetActivePostsRequest>
    {
        public GetActivePostValidtor() {
            RuleFor(x => x.StartStationId).GreaterThan(0);
            RuleFor(x => x.EndStationId).GreaterThan(0);

        }
    }
    public class GetActivePostsRequest : PaginationRequest<Post>, IRequest<PaginationResponse<PostResponse>>
    {
        [FromQuery]
        public string Query { get; set; } = "";
        [FromQuery]
        public long? StartStationId { get; set; }
        [FromQuery]
        public long? EndStationId { get; set; }
        [FromQuery]
        public string? AuthorEmail { get; set; }
        [FromQuery]
        public DateTimeOffset? StartFrom { get; set; }
        [FromQuery]
        public DateTimeOffset? StartTo { get; set; }
        [FromQuery]
        public string? Role { get; set; }
        [JsonIgnore]
        public string ExceptUserId { get; set; }
        [JsonIgnore]
        public PostStatus? Status = PostStatus.Created;
        public override Expression<Func<Post, bool>> GetExpressions()
        {
            Expression<Func<Post, bool>> expression = _ => true;
            expression = post =>
                post.Description.Contains(Query) &&
                StartStationId == null || post.StartStationId == StartStationId &&
                EndStationId == null || post.EndStationId == EndStationId &&
                AuthorEmail == null || post.Author.Email == AuthorEmail &&
                StartFrom == null || post.StartTime >= StartFrom &&
                StartTo == null || post.StartTime <= StartTo &&
                Role == null || post.TripRole.GetDescription().ToUpper() == Role &&
                /*ExceptUserId == null || post.AuthorId != Guid.Parse(ExceptUserId) &&*/
                Status == null || post.Status == Status
                ;
            return expression;
        }
    }
}
