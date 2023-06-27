using ClientService.Application.Common.Enums;
using ClientService.Application.Common.Extensions;
using ClientService.Application.Stations.Command;
using ClientService.Application.Stations.Model;
using ClientService.Domain.Common;
using ClientService.Domain.Wrappers;
using ClientService.Infrastructure.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ClientService.Application.Stations.Handler
{
    public class GetStationDetailHandler : IRequestHandler<GetStationDetailRequest, Response<StationDetailResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GetStationDetailHandler> _logger;

        public GetStationDetailHandler(
          IUnitOfWork unitOfWork,
          ILogger<GetStationDetailHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }


        public async Task<Response<StationDetailResponse>> Handle(GetStationDetailRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var stationQuery = await _unitOfWork.StationRepository.GetAsync(expression: x => x.Id == request.id, includeFunc: x => x.Include(station => station.PreviousStation).Include(station => station.NextStation), disableTracking:false);


                var station = stationQuery.FirstOrDefault();

                if (station == null)
                {
                    return new Response<StationDetailResponse>(code: (int)ResponseCode.StationErrorNotFound, message: ResponseCode.StationErrorNotFound.GetDescription());
                }


                return new Response<StationDetailResponse>(code: 0, data: new StationDetailResponse()
                {
                    Id = station.Id,
                    Name = station.Name,
                    Description = station.Description,
                    Address = station.Address,
                    Latitude = station.Latitude,
                    Longitude = station.Longitude,
                    Status = station.ObjectStatus.ToString().ToUpper(),
                    NextStations = station.PreviousStation.ConvertAll(value => new StationDetailResponse()
                    {
                        Id = value.Id,
                        Name = value.Name,
                        Description = value.Description,
                        Address = value.Address,
                        Latitude = value.Latitude,
                        Longitude = value.Longitude,
                        Status = value.ObjectStatus.ToString().ToUpper(),
                    })
                }
              );
            }
            catch (Exception ex)
            {
                return new Response<StationDetailResponse>(
                    code: (int)ResponseCode.Failed, message: ResponseCode.Failed.GetDescription());
            }
            finally
            {
            }
        }
    }
}
