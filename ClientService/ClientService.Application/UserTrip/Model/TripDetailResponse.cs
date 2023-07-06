using ClientService.Application.Accounts.Model;
using ClientService.Application.UserPost.Model;
using ClientService.Domain.Common;
using ClientService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientService.Application.UserTrip.Model
{
    public class UserTripDetailResponse
    {
        public long Id { get; set; }
        public TripStatus Status { get; set; }
        public string Description { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? StartAt { get; set; }
        public DateTimeOffset? FinishAt { get; set; }
        public DateTimeOffset? CancelAt { get; set; }
        public float? FeedbackPoint { get; set; }
        public string? FeedbackContent { get; set; }
        public long StartStationId { get; set; }
        public StationResponse StartStation { get; set; }
        public long EndStationId { get; set; }
        public StationResponse EndStation { get; set; }
        public DateTimeOffset? PostedStartTime { get; set; }
        public AccountResponse Passenger { get; set; }
        public AccountResponse Grabber { get; set; }
        public PostResponse Post { get; set; }
        public UserTripDetailResponse() { }
        public UserTripDetailResponse(Trip trip)
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
                StartStation = new StationResponse(sStation);
            }

            Station eStation = trip.EndStation;
            if (eStation != null)
            {
                EndStationId = eStation.Id;
                EndStation = new StationResponse(eStation);
            }

            Account aGrabber = trip.Grabber;
            if (aGrabber != null)
            {
                Grabber = new AccountResponse(aGrabber);
            }

            Account aPassenger = trip.Passenger;
            if (aPassenger != null)
            {
                Passenger = new AccountResponse(aPassenger);
            }

            Post tPost = trip.Post;
            if (tPost != null)
            {
                Post = new PostResponse(tPost);
            }
        }
    }
}
