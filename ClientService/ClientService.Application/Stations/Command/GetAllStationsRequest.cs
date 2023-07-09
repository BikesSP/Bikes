using ClientService.Application.Common.Models.Request;
using ClientService.Application.Common.Models.Response;
using ClientService.Application.Stations.Model;
using ClientService.Domain.Entities;
using ClientService.Domain.Wrappers;
using LinqKit;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ClientService.Application.Stations.Command
{
    public class GetAllStationsRequest : PaginationRequest<Station>, IRequest<PaginationResponse<StationDetailResponse>>
    {
        public long? FromStationId { get; set; }

        public override Expression<Func<Station, bool>> GetExpressions()
        {
            var expression = PredicateBuilder.New<Station>(true);

            if (FromStationId != null)
            {
                expression = expression.And(station => station.Id == FromStationId);
            }

            return expression;
        }
    }
}
