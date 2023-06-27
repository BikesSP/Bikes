using ClientService.Application.Common.Enums;
using ClientService.Application.Common.Extensions;
using ClientService.Application.Stations.Command;
using ClientService.Application.Stations.Model;
using ClientService.Domain.Common;
using ClientService.Domain.Entities;
using ClientService.Domain.Wrappers;
using ClientService.Infrastructure.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientService.Application.Stations.Handler
{
    public class UpdateStationHandler : IRequestHandler<UpdateStationRequest, Response<StationDetailResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UpdateStationHandler> _logger;

        public UpdateStationHandler(
            IUnitOfWork unitOfWork,
            ILogger<UpdateStationHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Response<StationDetailResponse>> Handle(UpdateStationRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var stationQuery = await _unitOfWork.StationRepository.GetAsync(expression: x => x.Id == request.Id);

                var station = stationQuery.FirstOrDefault();

                if (station == null)
                {
                    return new Response<StationDetailResponse>(code: (int)ResponseCode.StationErrorNotFound, message: ResponseCode.StationErrorNotFound.GetDescription());
                }

                var checkExistQuery = await _unitOfWork.TripRepository.GetAsync(expression: x => x.StartStationId == request.Id || x.EndStationId == request.Id);

                var result = checkExistQuery.FirstOrDefault();

                if (result != null)
                {
                    return new Response<StationDetailResponse>(code: (int)ResponseCode.StationErrorIsUsed, message: ResponseCode.StationErrorNotFound.GetDescription());
                }

                Station updateStation = new Station()
                {
                    Name = request.Name,
                    Address = request.Address,
                    Description = request.Description,
                    Latitude = (float)request.Latitude,
                    Longitude = (float)request.Longtitude,
                    ObjectStatus= station.ObjectStatus
                };

                await _unitOfWork.StationRepository.UpdateAsync(station);
                var updateResult = await _unitOfWork.SaveChangesAsync();

                return updateResult > 0 ?
                    new Response<StationDetailResponse>(code: 0, data: new StationDetailResponse()
                    {
                        Id = (int)updateStation.Id,
                        Name = updateStation.Name,
                        Address = updateStation.Address,
                        Description = updateStation.Description,
                        Latitude = updateStation.Latitude,
                        Longitude = updateStation.Longitude,
                        ObjectStatus = updateStation.ObjectStatus,
                    })
                    : new Response<StationDetailResponse>(code: (int)ResponseCode.Failed, message: ResponseCode.Failed.GetDescription());
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
