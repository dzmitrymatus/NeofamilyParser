using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NeofamilyParser.DAL.Models;
using NeofamilyParser.DAL.Repository;
using NeofamilyParser.WebAPI.Models;
using System.Linq.Expressions;

namespace NeofamilyParser.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IRepository<TaskEntity> _repository;

        public TasksController(IMapper mapper, IRepository<TaskEntity> repository) 
        {
            this._mapper = mapper;
            this._repository = repository;
        }

        [HttpGet("random")]
        public async Task<IResult> GetRandomTask(bool includeSolution = false)
        {
            return await this._repository.GetRandomAsync()
                .ContinueWith(task => 
                {
                    var result = _mapper.Map<TaskApiModel>(task.Result, opt => 
                                    opt.Items.Add("includeSolution", includeSolution));
                    return result;
                })
                .ContinueWith(task => Results.Json(task.Result));
        }

        [HttpGet("list")]
        public async Task<IResult> GetTasksList(string? section,
            string? theme,
            int? part,
            int? number,
            string? source, 
            bool includeSolution = false,
            int skip = 0,
            int take = int.MaxValue)
        {
            Expression<Func<IQueryable<TaskEntity>, IQueryable<TaskEntity>>> filterExpression
                = items => items.Where(item => section == null || item.Section == section)
                              .Where(item => theme == null || item.Theme == theme)
                              .Where(item => part == null || item.Part == part)
                              .Where(item => number == null || item.Number == number)
                              .Where(item => source == null || item.Source == source)
                              .Skip(skip)
                              .Take(take);

            return await this._repository.GetAllAsync(filterExpression)
                .ContinueWith(task => _mapper.Map<IEnumerable<TaskApiModel>>(task.Result, 
                                        opt => opt.Items.Add("includeSolution", includeSolution)))
                .ContinueWith(task => Results.Json(task.Result));
        }

        [HttpGet("solution/{taskId}")]
        public async Task<IResult> GetTaskSolution(int taskId)
        {
            return await this._repository.GetAsync(taskId)
                .ContinueWith(task => _mapper.Map<TaskSolutionApiModel>(task.Result))
                .ContinueWith(task => Results.Json(task.Result));
        }

        [HttpGet("{taskId}/checkAnswer/{answer}")]
        public async Task<IResult> CheckTaskSolution(int taskId, string answer)
        {
            return await this._repository.GetAsync(taskId)
                .ContinueWith(task => task.Result?.Answer == answer)
                .ContinueWith(task => Results.Json(task.Result));
        }

    }
}
