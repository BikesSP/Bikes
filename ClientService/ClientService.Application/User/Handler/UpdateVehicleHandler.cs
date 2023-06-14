using ClientService.Application.Common.Enums;
using ClientService.Application.Common.Extensions;
using ClientService.Application.Services.CurrentUserService;
using ClientService.Application.User.Command;
using ClientService.Application.User.Model;
using ClientService.Domain.Common;
using ClientService.Domain.Entities;
using ClientService.Domain.Wrappers;
using ClientService.Infrastructure.Repositories;
using Grpc.Core;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientService.Application.User.Handler
{
    public class UpdateVehicleHandler : IRequestHandler<UpdateVehicleRequest, Response<VehicleResponse?>>
    {
        private readonly ILogger<UpdateVehicleHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public UpdateVehicleHandler(
            ILogger<UpdateVehicleHandler> logger, IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }
        public async Task<Response<VehicleResponse?>> Handle(UpdateVehicleRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var vehicle = _currentUserService.GetCurrentAccount();
                if (vehicle == null)
                    return new Response<VehicleResponse?>(code: (int)ResponseCode.Failed, message: ResponseCode.Failed.GetDescription());

                vehicle.Color = request.Color;
                vehicle.LicensePlate = request.LicencePlate;
                vehicle.Brand = request.Brand;
                vehicle.ImageUrl = request.Image;
                vehicle.Description= request.Description;
                vehicle.Type = request.Type == "BIKE" ? VehicleType.Bike : VehicleType.Car;
                vehicle.Status = VehicleStatus.Waiting;

                _unitOfWork.AccountRepository.Update(vehicle);
                var result = await _unitOfWork.SaveChangesAsync();

                return result > 0 ? new Response<VehicleResponse?>(code: 0,
                        data: new VehicleResponse()
                        {
                            Id = vehicle.Id,
                            Color = vehicle.Color,
                            Brand = vehicle.Brand,
                            Description = vehicle.Description,
                            Image = vehicle.ImageUrl,
                            LicencePlate = vehicle.LicensePlate,
                            Type = vehicle.Type,
                            Status = vehicle.Status
                        }
                    ): new Response<VehicleResponse?>(code: (int)ResponseCode.Failed, message: ResponseCode.Failed.GetDescription());
            }
            catch (Exception ex)
            {
                return new Response<VehicleResponse?>(code: (int)ResponseCode.Failed, message: ResponseCode.Failed.GetDescription());
            }
            finally
            {
            }
        }
    }
}
