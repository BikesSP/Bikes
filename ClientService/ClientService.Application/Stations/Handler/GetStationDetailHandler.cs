﻿using ClientService.Application.Common.Enums;
using ClientService.Application.Common.Extensions;
using ClientService.Application.Stations.Command;
using ClientService.Application.Stations.Model;
using ClientService.Application.UserPost.Model;
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


        public async Task<Response<StationDetailResponse?>> Handle(GetStationDetailRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var stationQuery = await _unitOfWork.StationRepository.GetAsync(expression: x => x.Id == request.id);

                var station = stationQuery.FirstOrDefault();

                if (station == null)
                {
                    return new Response<StationDetailResponse?>(code: (int)ResponseCode.StationErrorNotFound, message: ResponseCode.StationErrorNotFound.GetDescription());
                }

                return new Response<StationDetailResponse?>(code: 0, data: new StationDetailResponse()
                {
                    Id = (int)station.Id,
                    Name = station.Name,
                    Description = station.Description,
                    Address = station.Address,
                    Latitude = station.Latitude,
                    Longitude = station.Longitude,
                    ObjectStatus = station.ObjectStatus,
                }
              );
            }
            catch (Exception ex)
            {
                return new Response<StationDetailResponse?>(
                    code: (int)ResponseCode.Failed, message: ResponseCode.Failed.GetDescription());
            }
            finally
            {
            }
        }
    }
}
