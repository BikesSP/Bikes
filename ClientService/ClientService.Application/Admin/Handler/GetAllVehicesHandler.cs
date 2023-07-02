using ClientService.Application.Admin.Command;
using ClientService.Application.Common.Enums;
using ClientService.Application.Common.Extensions;
using ClientService.Application.Common.Models.Response;
using ClientService.Application.Stations.Command;
using ClientService.Application.Stations.Handler;
using ClientService.Application.Stations.Model;
using ClientService.Application.User.Model;
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
    public class GetAllVehicesHandler : IRequestHandler<GetAllVehicesRequest, PaginationResponse<VehicleResponse>>
    {
        private readonly ILogger<GetAllVehicesHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public GetAllVehicesHandler(
            ILogger<GetAllVehicesHandler> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<PaginationResponse<VehicleResponse>> Handle(GetAllVehicesRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.AccountRepository.PaginationAsync(
                        page: request.PageNumber,
                        pageSize: request.PageSize,
                        filter: request.GetExpressions()
                    );
                return new PaginationResponse<VehicleResponse>(code: 0,
                    data: new PaginationData<VehicleResponse>()
                    {
                        Page = request.PageNumber,
                        PageSize = request.PageSize,
                        TotalSize = result.Total,
                        TotalPage = (int?)((result?.Total + (long)request.PageSize - 1) / (long)request.PageSize) ?? 0,
                        Items = result.Data.ConvertAll(vehicle => new VehicleResponse()
                        {
                            Id=vehicle.Id,
                            Brand=vehicle.Brand,
                            LicencePlate=vehicle.LicensePlate,
                            Color=vehicle.Color,
                            Image=vehicle.ImageUrl,
                            Description=vehicle.Description,
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
                        })
                    }
                    );
            }
            catch (Exception ex)
            {
                return new PaginationResponse<VehicleResponse>(code: (int)ResponseCode.Failed, message: ResponseCode.Failed.GetDescription());
            }
            finally
            {
            }
        }
    }
}
