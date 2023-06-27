using Amazon.Runtime.Internal;
using ClientService.Application.Common.Enums;
using ClientService.Application.Common.Extensions;
using ClientService.Application.Stations.Command;
using ClientService.Application.Stations.Model;
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

namespace ClientService.Application.Stations.Handler
{
    public class UpdateStationStatusHandler : IRequestHandler<UpdateStationStatusRequest, Response<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UpdateStationStatusHandler> _logger;

        public UpdateStationStatusHandler(
          IUnitOfWork unitOfWork,
          ILogger<UpdateStationStatusHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Response<bool>> Handle(UpdateStationStatusRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var stationQuery = await _unitOfWork.StationRepository.GetAsync(expression: x => x.Id == request.Id);

                var station = stationQuery.FirstOrDefault();

                if (station == null)
                {
                    return new Response<bool>(code: (int)ResponseCode.StationErrorNotFound, message: ResponseCode.StationErrorNotFound.GetDescription());
                }

                if (request.Status.Equals((ObjectStatus.Inactive))) {
                    var checkExistQuery = await _unitOfWork.TripRepository.GetAsync(expression: x => x.StartStationId == request.Id || x.EndStationId == request.Id, disableTracking: false);

                    var result = checkExistQuery.FirstOrDefault();

                    if (result != null)
                    {
                        return new Response<bool>(code: (int)ResponseCode.StationErrorIsUsed, message: ResponseCode.StationErrorNotFound.GetDescription());
                    }
                }

                station.ObjectStatus = request.Status;

                await _unitOfWork.StationRepository.UpdateAsync(station);
                var isSuccess = await _unitOfWork.SaveChangesAsync();

                return isSuccess > 0 ? new Response<bool>(code: 0, data: true) : new Response<bool>(
                code: (int)ResponseCode.Failed, message: ResponseCode.Failed.GetDescription());


            }
            catch (Exception ex)
            {
                return new Response<bool>(
                    code: (int)ResponseCode.Failed, message: ResponseCode.Failed.GetDescription());
            }
            finally
            {
            }

        }

    }
}
