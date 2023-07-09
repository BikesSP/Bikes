using ClientService.Application.Common.Models.Request;
using ClientService.Application.Stations.Model;
using ClientService.Application.User.Model;
using ClientService.Domain.Entities;
using ClientService.Domain.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ClientService.Application.Admin.Command
{
    public class GetAllVehicesRequest : PaginationRequest<Account>, IRequest<PaginationResponse<VehicleResponse>>
    {
        public override Expression<Func<Account, bool>> GetExpressions()
        {
            Expression<Func<Account, bool>> expression = _ => true;
            return expression;
        }
    }
}
