using ClientService.Application.Common.Models.Request;
using ClientService.Domain.Wrappers;
using ClientService.Domain.Entities;
using MediatR;
using ClientService.Domain.Common;
using LinqKit;
using System.Linq.Expressions;
using ClientService.Application.Trips.Model;

namespace ClientService.Application.Trips.Query
{
    public class GetAllTripRequest : PaginationRequest<Trip>, IRequest<PaginationResponse<TripResponse>>
    {
        public String? PassengerName { get; set; }
        public String? GrabberName { get; set; }
        public TripStatus? Status { get; set; }
        public String? StartStationName { get; set; }
        public String? EndStationName { get; set; }

        public override Expression<Func<Trip, bool>> GetExpressions()
        {
            var expression = PredicateBuilder.New<Trip>(true);

            if (PassengerName != null)
            {
                expression = expression.And(trip => trip.Passenger.Name.Contains(PassengerName));
            }

            if (GrabberName != null)
            {
                expression = expression.And(trip => trip.Grabber.Name.Contains(GrabberName));
            }

            if (Status != null)
            {
                expression = expression.And(trip => trip.TripStatus.Equals(Status));
            }

            if (StartStationName != null)
            {
                expression = expression.And(trip => trip.StartStation.Name.Contains(StartStationName));
            }

            if (EndStationName != null)
            {
                expression = expression.And(trip => trip.EndStation.Name.Contains(EndStationName));
            }

            return expression;
        }
    }
}
