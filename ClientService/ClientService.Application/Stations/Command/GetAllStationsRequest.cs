using ClientService.Application.Common.Models.Request;
using ClientService.Application.Common.Models.Response;
using ClientService.Application.Stations.Model;
using ClientService.Domain.Entities;
using ClientService.Domain.Wrappers;
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
        public override Expression<Func<Station, bool>> GetExpressions()
        {
            Expression<Func<Station, bool>> expression = _ => true;
            return expression;
        }
    }
}
