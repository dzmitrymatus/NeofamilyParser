using AutoMapper;
using NeofamilyParser.BLL.Models;
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
                .ForMember(dest => dest.Solution, opt =>
                    {
                        opt.Condition((src, dest, x, y, context) => (bool)context.Items["includeSolution"] == true);
                        opt.MapFrom(src => src);
                    })
                .ReverseMap();

            CreateMap<TaskModel, TaskEntity>()
                .ForMember(dest => dest.QuestionImages, opt =>
                    opt.MapFrom(x => string.Join(",", x.QuestionImages.Select(y => $"Images/Question/{y.Key}"))))
                .ForMember(dest => dest.SolutionImages, opt =>
                    opt.MapFrom(x => string.Join(",", x.SolutionImages.Select(y => $"Images/Solution/{y.Key}"))))
                .ReverseMap();
        }
    }
}
