using ClientService.Application.Admin.Command;
using ClientService.Application.Common.Enums;
using ClientService.Application.Common.Extensions;
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
    public class GetAllAccountsHandler : IRequestHandler<GetAllAccountsRequest, PaginationResponse<UserProfileResponse>>
    {
        private readonly ILogger<GetAllAccountsHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public GetAllAccountsHandler(
            ILogger<GetAllAccountsHandler> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<PaginationResponse<UserProfileResponse>> Handle(GetAllAccountsRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.AccountRepository.PaginationAsync(
                        page: request.PageNumber,
                        pageSize: request.PageSize,
                        filter: request.GetExpressions()
                    );
                return new PaginationResponse<UserProfileResponse>(code: 0,
                    data: new PaginationData<UserProfileResponse>()
                    {
                        Page = request.PageNumber,
                        PageSize = request.PageSize,
                        TotalSize = result.Total,
                        TotalPage = (int?)((result?.Total + (long)request.PageSize - 1) / (long)request.PageSize) ?? 0,
                        Items = result.Data.ConvertAll(account => new UserProfileResponse()
                        {
                          Id=account.Id.ToString(),
                          Name=account.Name,
                          Email=account.Email,
                          Avatar=account.AvartarUlr,
                          AveragePoint=account.averagePoint,
                          IsUpdated=account.IsUpdated,
                          Phone=account.Phone,
                          Status=account.AccountStatus.ToString().ToUpper(),
                        })
                    }
                    );
            }
            catch (Exception ex)
            {
                return new PaginationResponse<UserProfileResponse>(code: (int)ResponseCode.Failed, message: ResponseCode.Failed.GetDescription());
            }
            finally
            {
            }
        }
    }
}
