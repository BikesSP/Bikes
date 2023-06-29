using ClientService.Domain.Common;
using Microsoft.Extensions.FileSystemGlobbing.Internal;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ClientService.Application.Trips.Model
{
    public class TripResponse
    {
        public long Id { get; set; }
        public Guid PassengerId { get; set; }
        public String PassengerName { get; set; }
        public Guid GrabberId { get; set; }
        public String GrabberName { get; set; }
        public DateTimeOffset StartTime { get; set; }

        public DateTimeOffset EndTime { get; set; }
        public DateTimeOffset CancelTime { get; set; }
        public float? FeedbackPoint { get; set; }
        public String FeedbackContent { get; set; }
        public TripStatus Status { get; set; }
        public long StartStationId { get; set; }
        public String StartStationName { get; set; }
        public long EndStationId { get; set; }
        public String EndStationName { get; set; }
        public DateTimeOffset PostedStartTime { get; set; }
    }
}
