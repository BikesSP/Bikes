using ClientService.Application.Services.CurrentUserService;
using ClientService.Application.User.Command;
using ClientService.Application.User.Model;
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
                var currentUser = _currentUserService.GetCurrentAccount();
                if(currentUser == null)
                    return new Response<VehicleResponse?>(code: -1, message: "Internal server error");

              
                
                if(currentUser.LicensePlate == null) 
                    return new Response<VehicleResponse?>(code: 61,
                        data: null
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
                            Status = currentUser.Status
                        }
                    );
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
