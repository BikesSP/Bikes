using ClientService.Application.Common.Models.Request;
using ClientService.Application.UserTrip.Model;
using ClientService.Domain.Common;
using ClientService.Domain.Entities;
using ClientService.Domain.Wrappers;
using LinqKit;
using MediatR;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ClientService.Application.UserTrip.Query
{
    public class GetAllUserTripRequest : PaginationRequest<Trip>, IRequest<PaginationResponse<UserTripResponse>>
    {
        public Guid UserId { get; set; }
        public string? Query { get; set; }

        public long? StartStationId { get; set; }

        public long? EndStationId { get; set; }

        public TripStatus? Status { get; set; }
        public DateTimeOffset? StartFrom { get; set; }

        public DateTimeOffset? StartTo { get; set; }
        public override Expression<Func<Trip, bool>> GetExpressions()
        {
            var expression = PredicateBuilder.New<Trip>(true);

            if (!String.IsNullOrEmpty(Query))
            {
                expression = expression.And(trip => trip.Description.Contains(Query.Trim()));
            }

            if (StartStationId != null)
            {
                expression = expression.And(trip => trip.StartStation.Id.Equals(StartStationId));
            }

            if (EndStationId != null)
            {
                expression = expression.And(trip => trip.EndStation.Id.Equals(EndStationId));
            }

            if (StartFrom != null)
            {
                expression = expression.And(trip => StartFrom.Value.ToUniversalTime().CompareTo(trip.StartAt.Value) <= 0);
            }

            if (StartTo != null)
            {
                expression = expression.And(trip => StartTo.Value.ToUniversalTime().CompareTo(trip.StartAt.Value) >= 0);
            }

            if (Status != null)
            {
                expression = expression.And(trip => trip.TripStatus.Equals(Status));
            }
            
            var queryException = PredicateBuilder.New<Trip>();
            queryException = queryException.Or(trip => trip.Grabber.Id.Equals(UserId));
            queryException = queryException.Or(trip => trip.Passenger.Id.Equals(UserId));
            expression = expression.And(queryException);
            return expression;
        }
    }
}
