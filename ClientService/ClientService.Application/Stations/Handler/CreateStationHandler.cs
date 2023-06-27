using AutoMapper;
using ClientService.Application.Common.Enums;
using ClientService.Application.Common.Extensions;
using ClientService.Application.Common.Models.Response;
using ClientService.Application.Stations.Command;
using ClientService.Application.Stations.Model;
using ClientService.Application.UserPost.Model;
using ClientService.Domain.Common;
using ClientService.Domain.Entities;
using ClientService.Domain.Wrappers;
using ClientService.Infrastructure.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientService.Application.Stations.Handler
{
    public class CreateStationHandler : IRequestHandler<CreateStationRequest, Response<StationDetailResponse?>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CreateStationHandler> _logger;

        public CreateStationHandler(
            IUnitOfWork unitOfWork,
            ILogger<CreateStationHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Response<StationDetailResponse?>> Handle(CreateStationRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var nextStationQuery = await _unitOfWork.StationRepository.GetAllAsync(expression: x => request.NextStationsIds.Contains(x.Id) && x.ObjectStatus == ObjectStatus.Active, disableTracking:false);
                var nextStations = nextStationQuery.ToList();

                if (request.NextStationsIds.Count != 0 && nextStations.ToList().Count() == 0)
                {
                    return new Response<StationDetailResponse?>(code: (int)ResponseCode.StationErrorIsInactive, message: ResponseCode.StationErrorIsInactive.GetDescription());
                }


                Station station = new Station()
                {
                    Name = request.Name,
                    Address=request.Address,
                    Description = request.Description,
                    Latitude = request.Latitude,
                    Longitude = request.Longitude,
                    ObjectStatus = ObjectStatus.Active,
                    NextStation=nextStations.ToList()
                };

                await _unitOfWork.StationRepository.AddAsync(station);
                var result = await _unitOfWork.SaveChangesAsync();

                return result > 0 ? 
                    new Response<StationDetailResponse?>(code: 0, data: new StationDetailResponse()
                    {
                        Id= station.Id,
                        Name=station.Name,
                        Address=station.Address,
                        Description=station.Description,
                        Latitude=station.Latitude,
                        Longitude=station.Longitude,
                        Status= ObjectStatus.Active.ToString().ToUpper(),
                    })
                    : new Response<StationDetailResponse?>(code: (int)ResponseCode.Failed, message: ResponseCode.Failed.GetDescription());
            }
            catch (Exception ex)
            {
                return new Response<StationDetailResponse?>(code: (int)ResponseCode.Failed, message: ResponseCode.Failed.GetDescription());
            }
            finally
            {
            }
        }
    }

}
