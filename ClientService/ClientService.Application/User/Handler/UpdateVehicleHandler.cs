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

        public UpdateVehicleHandler(
            ILogger<UpdateVehicleHandler> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }
        public async Task<Response<VehicleResponse?>> Handle(UpdateVehicleRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var vehicles = _unitOfWork.VehicleRepository.GetAll(
                    expression: vehicle => vehicle.Account.Email == request.ownerEmail,
                    includeFunc: x => x.Include(vehicle => vehicle.Account));

                if(vehicles.Count() == 0 && !request.isCreatedIfNull)
                {
                    return new Response<VehicleResponse?>(code: 80, message: "No vehicle to update");
                }

                if(vehicles.Any(x => x.Status == VehicleStatus.Waiting))
                {
                    return new Response<VehicleResponse?>(code: 80, message: "One of your vehicle is waiting for confirm");
                }

                var user = _unitOfWork.AccountRepository.FirstOrDefault(expression: account => account.Email == request.ownerEmail);
                if (user == null)
                {
                    throw new Exception("Internal server error");
                }
                Vehicle vehicle = new Vehicle()
                {
                    AccountId = user.Id,
                    Color = request.Color,
                    Brand = request.Brand,
                    Description = request.Description,
                    ImageUrl = request.Image,
                    LicensePlate = request.Image,
                    Type = request.Type,
                    Status = VehicleStatus.Waiting
                };
                _unitOfWork.VehicleRepository.Add(vehicle);

                var result = await _unitOfWork.SaveChangesAsync();

                return result > 0 ? new Response<VehicleResponse?>(code: 0, data: new VehicleResponse()
                {
                    Id = vehicle.Id,
                    Color = vehicle.Color,
                    Brand = vehicle.Brand,
                    Description = vehicle.Description,
                    Image = vehicle.ImageUrl,
                    LicencePlate = vehicle.LicensePlate,
                    Type = vehicle.Type,
                    Status = vehicle.Status
                }) :
                new Response<VehicleResponse?>(code: -1, message: "Internal server error")
                ;

            }
            catch (Exception ex)
            {
                return new Response<VehicleResponse?>(code: -1, message: "Internal server error");
            }
            finally
            {
            }
        }
    }
}
