using ClientService.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientService.Domain.Entities
{
    public class Trip : BaseAuditableEntity 
    {
        public long Id { get; set; }
        public string Description { get; set; }
        [ForeignKey("PassengerId")]
        public Account Passenger { get; set; }
        [ForeignKey("GrabberId")]
        public Account Grabber { get; set; }
        public Guid? PassengerId { get; set; }
        public Guid? GrabberId { get; set; }
        public TripStatus TripStatus { get; set; }
        public DateTimeOffset StartAt { get; set; }
        public DateTimeOffset FinishAt { get; set;}
        public DateTimeOffset CancelAt { get; set; }
        public DateTimeOffset PostedStartTime { get; set; }
        public float FeedbackPoint { get; set; }
        public string FeedbackContent { get; set; }
        [ForeignKey("StartStationId")]
        public Station StartStation { get; set; }
        [ForeignKey("EndStationId")]
        public Station EndStation { get; set; }
        public long StartStationId { get; set; }
        public long EndStationId { get; set;}
        [ForeignKey("Id")]
        public Post Post { get; set; }
    }
}
