using ClientService.Domain.Common;
using ClientService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientService.Application.UserPost.Model
{
    public class PostResponse
    {
        public long Id { get; set; }
        public string TripRole { get; set; }
        public string Description { get; set; }
        public Guid AuthorId { get; set; }
        public string AuthorName { get; set; }
        public DateTimeOffset StartTime { get; set; }
        public string Status { get; set; }
        public float FeedbackPoint { get; set; }
        public string FeedbackContent { get; set; }
        public long StartStationId { get; set; }
        public string StartStation { get; set; }
        public long EndStationId { get; set; }
        public string EndStation { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
    }
}
