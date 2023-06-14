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
    public class UpdateVehicleStatusHandler : IRequestHandler<UpdateVehicleStatusRequest, Response<bool>>
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

        public async Task<Response<bool>> Handle(UpdateVehicleStatusRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var vehicle = _unitOfWork.AccountRepository.FirstOrDefault(x => x.Id.ToString() == request.Id && x.Status == Domain.Common.VehicleStatus.Waiting);
                if (vehicle == null)
                {
                    return new Response<bool>(code: -1, message: "Invalid vehicle");
                }

                vehicle.Status = request.Approved ? Domain.Common.VehicleStatus.Approved : Domain.Common.VehicleStatus.Deny;

                _unitOfWork.AccountRepository.Update(vehicle);

                var result = await _unitOfWork.SaveChangesAsync();

                return new Response<bool>(code: 0, data: result > 0);
            }
            catch (Exception ex)
            {
                return new Response<bool>(code: -1, message: "Internal server error");
            }
            finally
            {
            }
        }
    }
}
