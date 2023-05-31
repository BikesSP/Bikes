using AutoMapper;
using ClientService.Application.Common.Interfaces;
using ClientService.Application.Common.Models.Response;
using ClientService.Application.Stations.Command;
using ClientService.Application.Stations.Model;
using ClientService.Domain.Entities;
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
    public class CreateStationHandler : IRequestHandler<CreateStationRequest, StationDetailResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CreateStationHandler> _logger;
        private readonly IMapper _mapper;

        public CreateStationHandler(
            IUnitOfWork unitOfWork,
            ILogger<CreateStationHandler> logger,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<StationDetailResponse> Handle(CreateStationRequest request, CancellationToken cancellationToken)
        {
            var station = _mapper.Map<Station>(request);
            await _unitOfWork.StationRepository.AddAsync(station);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<StationDetailResponse>(station);
        }
    }

}
