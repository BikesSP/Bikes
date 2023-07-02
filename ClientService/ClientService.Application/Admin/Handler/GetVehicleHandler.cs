using ClientService.Application.Common.Enums;
using ClientService.Application.Common.Extensions;
using ClientService.Application.Stations.Handler;
using ClientService.Application.Stations.Model;
using ClientService.Application.User.Model;
using ClientService.Application.Vehicles.Command;
using ClientService.Domain.Wrappers;
using ClientService.Infrastructure.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientService.Application.Vehicles.Handler
{
    public class GetVehicleHandler : IRequestHandler<GetVehicleRequest, Response<VehicleResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GetVehicleHandler> _logger;

        public GetVehicleHandler(
          IUnitOfWork unitOfWork,
          ILogger<GetVehicleHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Response<VehicleResponse>> Handle(GetVehicleRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var vehicleQuery = await _unitOfWork.AccountRepository.GetAsync(expression: x => x.Id.ToString() == request.id);

                var vehicle = vehicleQuery.FirstOrDefault();

                if (vehicle == null)
                {
                    return new Response<VehicleResponse>(code: (int)ResponseCode.StationErrorNotFound, message: ResponseCode.StationErrorNotFound.GetDescription());
                }
                return new Response<VehicleResponse>(code: 0, data: new VehicleResponse()
                {
                    Id=vehicle.Id,
                    Description=vehicle.Description,
                    LicencePlate=vehicle.LicensePlate,
                    Image=vehicle.ImageUrl,
                    Brand=vehicle.Brand,
                    Color=vehicle.Color,
                    Type=vehicle.Type,
                    Status=vehicle.Status.ToString().ToUpper(),
                    Owner = new UserProfileResponse()
                    {
                        Id = vehicle.Id.ToString(),
                        Email = vehicle.Email,
                        Name = vehicle.Name,
                        Phone = vehicle.Phone,
                        Avatar = vehicle.AvartarUlr,
                        AveragePoint = vehicle.averagePoint,
                        Status = vehicle.Status.ToString().ToUpper(),
                        IsUpdated = vehicle.IsUpdated,
                    }
                });
            }
            catch (Exception ex)
            {
                return new Response<VehicleResponse>(
                    code: (int)ResponseCode.Failed, message: ResponseCode.Failed.GetDescription());
            }
            finally
            {
            }
        }
    }
}
