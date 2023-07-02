using ClientService.Application.Admin.Command;
using ClientService.Application.Common.Enums;
using ClientService.Application.Common.Extensions;
using ClientService.Application.Stations.Handler;
using ClientService.Application.Stations.Model;
using ClientService.Application.User.Model;
using ClientService.Domain.Wrappers;
using ClientService.Infrastructure.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientService.Application.Admin.Handler
{
    public class GetAccountDetailHandler : IRequestHandler<GetAccountDetailRequest, Response<UserProfileResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GetAccountDetailHandler> _logger;

        public GetAccountDetailHandler(
          IUnitOfWork unitOfWork,
          ILogger<GetAccountDetailHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Response<UserProfileResponse>> Handle(GetAccountDetailRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var accountQuery = await _unitOfWork.AccountRepository.GetAsync(expression: x => x.Id.ToString() == request.Id, disableTracking:true);


                var account = accountQuery.FirstOrDefault();

                if (account == null)
                {
                    return new Response<UserProfileResponse>(code: (int)ResponseCode.AccountErrorNotFound, message: ResponseCode.AccountErrorNotFound.GetDescription());
                }


                return new Response<UserProfileResponse>(code: 0, data: new UserProfileResponse()
                {
                    Id=account.Id.ToString(),
                    Email=account.Email,
                    Name=account.Name,
                    Phone=account.Phone,
                    Avatar=account.AvartarUlr,
                    AveragePoint=account.averagePoint,
                    IsUpdated=account.IsUpdated,
                    Status=account.Status.ToString().ToUpper(),
                }
              );
            }
            catch (Exception ex)
            {
                return new Response<UserProfileResponse>(
                    code: (int)ResponseCode.Failed, message: ResponseCode.Failed.GetDescription());
            }
            finally
            {
            }
        }
    }
}
