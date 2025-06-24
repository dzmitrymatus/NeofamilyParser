using Microsoft.AspNetCore.Mvc;
using NeofamilyParser.BLL.ApiClient;

namespace NeofamilyParser.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParsingController : ControllerBase
    {
        [HttpPost("parse")]
        public async Task<IResult> Parse()
        {
            var parser = new NeofamilyApiClient();
            var tasks = parser.GetTasks();

            return Results.Ok();
        }
    }
}
