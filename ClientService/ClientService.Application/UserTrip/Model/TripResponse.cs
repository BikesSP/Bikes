using ClientService.Domain.Common;
using ClientService.Domain.Entities;
using Microsoft.Extensions.FileSystemGlobbing.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ClientService.Application.UserTrip.Model
{
    public class UserTripResponse
    {
        public long Id { get; set; }
        public TripStatus Status { get;set; }
        public string Description { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset StartAt { get; set; }
        public DateTimeOffset FinishAt { get; set; }
        public DateTimeOffset CancelAt { get; set; }
        public float? FeedbackPoint { get; set; }
        public string? FeedbackContent { get; set; }
        public long StartStationId { get; set; }
        public String StartStation { get; set; }
        public long EndStationId { get; set; }
        public String EndStation { get; set; }
        public DateTimeOffset PostedStartTime { get; set; }
        public Guid PassengerId { get; set; }
        public String PassengerName { get; set; }
        public Guid GrabberId { get; set; }
        public String GrabberName { get; set; }

        public UserTripResponse() { }

        public UserTripResponse(Trip trip)
        {
            this.Id = trip.Id;
            this.Status = trip.TripStatus;
            this.Description = trip.Description;
            this.CreatedAt = trip.CreatedAt;
            this.StartAt = trip.StartAt;
            this.FinishAt = trip.FinishAt;
            this.CancelAt = trip.CancelAt;
            this.FeedbackPoint = trip.FeedbackPoint;
            this.FeedbackContent = trip.FeedbackContent;
            this.PostedStartTime = trip.PostedStartTime;

            Station sStation = trip.StartStation;
            if (sStation != null)
            {
                StartStationId = sStation.Id;
                StartStation = sStation.Name;
            }

            Station eStation = trip.EndStation;
            if (eStation != null)
            {
                EndStationId = eStation.Id;
                EndStation = eStation.Name;
            }

            Account grabber = trip.Grabber;
            if (grabber != null)
            {
                GrabberId = grabber.Id;
                GrabberName = grabber.Name;
            }
            Account passenger = trip.Passenger;
            if (passenger != null)
            {
                PassengerId = passenger.Id;
                PassengerName = passenger.Name;
            }
        }
    }
}
