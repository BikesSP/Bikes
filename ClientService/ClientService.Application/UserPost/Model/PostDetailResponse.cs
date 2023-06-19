using ClientService.Application.User.Model;
using ClientService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientService.Application.UserPost.Model
{
    public class PostDetailResponse
    {
        public long Id { get; set; }
        public string Role { get; set; }
        public string Description { get; set; }
        public UserProfileResponse Author { get; set; }
        public DateTimeOffset StartTime { get; set; }
        public string Status { get; set; }
        public float? FeedbackPoint { get; set; }
        public string? FeedbackContent { get; set; }
        //TODO: will replace later
        public Station StartStation { get; set; }
        //TODO: will replace later
        public Station EndStation { get; set; }
        public List<UserProfileResponse> Applications { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
    }
}
