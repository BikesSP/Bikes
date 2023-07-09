using ClientService.Application.Common.Enums;
using ClientService.Application.Common.Extensions;
using ClientService.Application.Services.CurrentUserService;
using ClientService.Application.User.Command;
using ClientService.Application.User.Model;
using ClientService.Domain.Entities;
using ClientService.Domain.Wrappers;
using ClientService.Infrastructure.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ClientService.Application.User.Handler
{
    public class GetCurrentUserVehicleHandler : IRequestHandler<GetCurrentUserVehicleRequest, Response<VehicleResponse?>>
    {
        private readonly ILogger<GetCurrentUserVehicleHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public GetCurrentUserVehicleHandler(
            ILogger<GetCurrentUserVehicleHandler> logger, IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<Response<VehicleResponse?>> Handle(GetCurrentUserVehicleRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var currentUser = await _currentUserService.GetCurrentAccount();
                if(currentUser == null)
                    return new Response<VehicleResponse?>(code: (int)ResponseCode.AccountErrorNotFound, message: ResponseCode.AccountErrorNotFound.GetDescription());

              
                
                if(currentUser.LicensePlate == null) 
                    return new Response<VehicleResponse?>(code: (int)ResponseCode.VehicleErrorNotFound,
                        message: ResponseCode.VehicleErrorNotFound.GetDescription()
                    );

                return new Response<VehicleResponse?>(code: 0,
                        data: new VehicleResponse()
                        {
                            Id = currentUser.Id,
                            Color = currentUser.Color,
                            Brand = currentUser.Brand,
                            Description = currentUser.Description,
                            Image = currentUser.ImageUrl,
                            LicencePlate = currentUser.LicensePlate,
                            Type = currentUser.Type,
                            Status = currentUser.Status.ToString().ToUpper(),
                            Owner = new UserProfileResponse()
                            {
                                Id = currentUser.Id.ToString(),
                                Email = currentUser.Email,
                                Name = currentUser.Name,
                                Phone = currentUser.Phone,
                                Avatar = currentUser.AvartarUlr,
                                AveragePoint = currentUser.averagePoint,
                                Status = currentUser.Status.ToString().ToUpper(),
                                IsUpdated = currentUser.IsUpdated,
                            }
                        }
                    );
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
