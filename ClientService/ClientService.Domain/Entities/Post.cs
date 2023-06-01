using ClientService.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientService.Domain.Entities
{
    public class Post : BaseAuditableEntity
    {
        public long Id { get; set; }
        public string Description { get; set; }
        [ForeignKey("AuthorId")]
        public Account Author { get; set; }
        public Guid AuthorId { get; set; }
        public Role TripRole { get; set; }
        public DateTimeOffset StartTime { get; set; }
        public PostStatus Status { get; set; }
        [InverseProperty("Post")]
        public Trip? Trip { get; set; }
        public virtual List<Account> Applier { get; set; } 
        public float FeedbackPoint { get; set; }
        public string FeedbackContent { get; set; }
        [ForeignKey("StartStationId")]
        public Station StartStation { get; set; }
        [ForeignKey("EndStationId")]
        public Station EndStation { get; set; }
        public long StartStationId { get; set; }
        public long EndStationId { get; set; }
    }
}
