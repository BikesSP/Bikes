using ClientService.Application.Common.Enums;
using ClientService.Application.Common.Extensions;
using ClientService.Application.Common.Models.Response;
using ClientService.Application.Services.CurrentUserService;
using ClientService.Application.Stations.Command;
using ClientService.Application.Stations.Model;
using ClientService.Application.UserPost.Command;
using ClientService.Application.UserPost.Handler;
using ClientService.Application.UserPost.Model;
using ClientService.Domain.Common;
using ClientService.Domain.Wrappers;
using ClientService.Infrastructure.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientService.Application.Stations.Handler
{
    public class GetAllStationsHandler : IRequestHandler<GetAllStationsRequest, PaginationResponse<StationDetailResponse>>
    {
        private readonly ILogger<GetAllStationsHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public GetAllStationsHandler(
            ILogger<GetAllStationsHandler> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<PaginationResponse<StationDetailResponse>> Handle(GetAllStationsRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.StationRepository.PaginationAsync(
                        page: request.PageNumber,
                        pageSize: request.PageSize,
                        filter: request.GetExpressions(),
                        includeFunc: query => query.Include(station => station.NextStation)
                    );

                return new PaginationResponse<StationDetailResponse>(code: 0,
                    data: new PaginationData<StationDetailResponse>()
                    {
                        Page = request.PageNumber,
                        PageSize = request.PageSize,
                        TotalSize = result.Total,
                        TotalPage = (int?)((result?.Total + (long)request.PageSize - 1) / (long)request.PageSize) ?? 0,
                        Items = request.FromStationId == null ? result.Data.ConvertAll(station => new StationDetailResponse()
                        {
                            Id = station.Id,
                            Name = station.Name,
                            Description = station.Description,
                            Address = station.Address,
                            Latitude = station.Latitude,
                            Longitude = station.Longitude,
                            Status = station.ObjectStatus.ToString().ToUpper(),
                        }) : result.Total != 0 ? result.Data.FirstOrDefault().NextStation.ConvertAll(station => new StationDetailResponse()
                        {
                            Id = station.Id,
                            Name = station.Name,
                            Description = station.Description,
                            Address = station.Address,
                            Latitude = station.Latitude,
                            Longitude = station.Longitude,
                            Status = station.ObjectStatus.ToString().ToUpper(),
                        }) : new List<StationDetailResponse>()
                    }
                    );
            }
            catch (Exception ex)
            {
                return new PaginationResponse<StationDetailResponse>(code: (int)ResponseCode.Failed, message: ResponseCode.Failed.GetDescription());
            }
            finally
            {
            }
        }
    }
}
