using AutoMapper;
using ClientService.Application.Stations.Command;
using ClientService.Application.Stations.Model;
using ClientService.Domain.Entities;

namespace ClientService.Application.Common.Mappings
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            // Major
            CreateMap<Station, StationDetailResponse>();
            CreateMap<CreateStationRequest, Station>();
        }
    }

}
