using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NeofamilyParser.BLL.ApiClient;
using NeofamilyParser.DAL.Models;
using NeofamilyParser.DAL.Repository;

namespace NeofamilyParser.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParsingController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        private readonly IMapper _mapper;
        private readonly IRepository<TaskEntity> _repository;
        private readonly INeofamilyApiClient _apiClient;

        public ParsingController(IWebHostEnvironment env, 
            IMapper mapper, 
            IRepository<TaskEntity> repository,
            INeofamilyApiClient apiClient)
        {
            this._env = env;
            this._mapper = mapper;
            this._repository = repository;
            this._apiClient = apiClient;
        }

        [HttpPost("parse")]
        public async Task<IResult> Parse()
        {
            var tasks = this._apiClient.GetTasks();

            //save images
            foreach (var task in tasks)
            {
                foreach (var image in task.QuestionImages)
                {
                    string path = Path.Combine(_env.WebRootPath, "Images", "Question", image.Key);
                    System.IO.File.WriteAllBytes(path, image.Value);
                }
                foreach (var image in task.SolutionImages)
                {
                    string path = Path.Combine(_env.WebRootPath, "Images", "Solution", image.Key);
                    System.IO.File.WriteAllBytes(path, image.Value);
                }
            }

            var taskEntities = this._mapper.Map<IEnumerable<TaskEntity>>(tasks);
            //save to db
            await this._repository.InsertRangeAsync(taskEntities);

            return Results.Ok();
        }
    }
}
