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

            if(tasks.Any())
            {
                //cleanup
                Directory.EnumerateFiles(Path.Combine(_env.WebRootPath, "Images", "Question"))
                    .ToList()
                    .ForEach(System.IO.File.Delete);
                Directory.EnumerateFiles(Path.Combine(_env.WebRootPath, "Images", "Solution"))
                    .ToList()
                    .ForEach(System.IO.File.Delete);

                await _repository.DeleteAllAsync();
            }

            //save images
            foreach (var task in tasks)
            {
                foreach (var image in task.QuestionImages)
                {
                    string path = Path.Combine(_env.WebRootPath, "Images", "Question", image.Name);
                    System.IO.File.WriteAllBytes(path, image.Image);
                }
                foreach (var image in task.SolutionImages)
                {
                    string path = Path.Combine(_env.WebRootPath, "Images", "Solution", image.Name);
                    System.IO.File.WriteAllBytes(path, image.Image);
                }
            }

            var taskEntities = this._mapper.Map<IEnumerable<TaskEntity>>(tasks);
            //save to db
            await this._repository.InsertRangeAsync(taskEntities);

            return Results.Ok();
        }
    }
}
