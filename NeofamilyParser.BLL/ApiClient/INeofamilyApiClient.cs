using NeofamilyParser.BLL.Models;

namespace NeofamilyParser.BLL.ApiClient
{
    public interface INeofamilyApiClient
    {
        IEnumerable<TaskModel> GetTasks();
    }
}
