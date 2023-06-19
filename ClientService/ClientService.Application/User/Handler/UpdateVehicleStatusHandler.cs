using ClientService.Application.Common.Enums;
using ClientService.Application.Common.Extensions;
using ClientService.Application.Common.Models.Response;
using ClientService.Application.Services.CurrentUserService;
using ClientService.Application.User.Command;
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

namespace ClientService.Application.User.Handler
{
    public class UpdateVehicleStatusHandler : IRequestHandler<UpdateVehicleStatusRequest, Response<BaseBoolResponse>>
    {
        private readonly ILogger<UpdateVehicleStatusHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public UpdateVehicleStatusHandler(
            ILogger<UpdateVehicleStatusHandler> logger, IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<Response<BaseBoolResponse>> Handle(UpdateVehicleStatusRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var vehicleQuery = await _unitOfWork.AccountRepository.GetAsync(expression: x => x.Id.ToString() == request.Id && x.Status == Domain.Common.VehicleStatus.Waiting);
                var vehicle = vehicleQuery.FirstOrDefault();
                if (vehicle == null)
                {
                    return new Response<BaseBoolResponse>(code: (int)ResponseCode.AccountErrorNotFound, message: ResponseCode.AccountErrorNotFound.GetDescription());
                }

                vehicle.Status = request.Approved ? Domain.Common.VehicleStatus.Approved : Domain.Common.VehicleStatus.Deny;

                await _unitOfWork.AccountRepository.UpdateAsync(vehicle);

                var result = await _unitOfWork.SaveChangesAsync();

                return new Response<BaseBoolResponse>(code: 0, data: new BaseBoolResponse() { Success = result > 0 });
            }
            catch (Exception ex)
            {
                return new Response<BaseBoolResponse>(code: (int)ResponseCode.Failed, message: ResponseCode.Failed.GetDescription());
            }
            finally
            {
            }
        }
    }
}
