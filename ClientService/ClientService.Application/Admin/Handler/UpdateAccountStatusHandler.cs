using ClientService.Application.Admin.Command;
using ClientService.Application.Common.Enums;
using ClientService.Application.Common.Extensions;
using ClientService.Domain.Common;
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
    public class UpdateAccountStatusHandler : IRequestHandler<UpdateAccountStatusRequest, Response<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UpdateAccountStatusHandler> _logger;

        public UpdateAccountStatusHandler(
          IUnitOfWork unitOfWork,
          ILogger<UpdateAccountStatusHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Response<bool>> Handle(UpdateAccountStatusRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var accountQuery = await _unitOfWork.AccountRepository.GetAsync(expression: x => x.Id.ToString() == request.Id);

                var account = accountQuery.FirstOrDefault();

                if (account == null)
                {
                    return new Response<bool>(code: (int)ResponseCode.StationErrorNotFound, message: ResponseCode.StationErrorNotFound.GetDescription());
                }


                account.AccountStatus = request.Status.ToUpper() == "ACTIVE" ? ObjectStatus.Active : ObjectStatus.Inactive;

                await _unitOfWork.AccountRepository.UpdateAsync(account);
                var isSuccess = await _unitOfWork.SaveChangesAsync();

                return isSuccess > 0 ? new Response<bool>(code: 0, data: true) : new Response<bool>(
                code: (int)ResponseCode.Failed, message: ResponseCode.Failed.GetDescription());


            }
            catch (Exception ex)
            {
                return new Response<bool>(
                    code: (int)ResponseCode.Failed, message: ResponseCode.Failed.GetDescription());
            }
            finally
            {
            }

        }
    }
}
