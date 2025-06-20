using AutoMapper;
using NeofamilyParser.DAL.Models;
using NeofamilyParser.WebAPI.Models;

namespace NeofamilyParser.WebAPI.Configuration
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<TaskEntity, TaskSolutionApiModel>()
                .ForMember(dest => dest.TaskId, opt => opt.MapFrom(src => src.Id))
                .ReverseMap();

            CreateMap<TaskEntity, TaskApiModel>()
                .ReverseMap();
        }
    }
}
