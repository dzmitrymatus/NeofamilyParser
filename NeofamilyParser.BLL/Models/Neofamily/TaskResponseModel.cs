using System.Text.Json.Serialization;

namespace NeofamilyParser.BLL.Models.Neofamily
{
    internal class TaskResponseModel
    {
        public int Id { get; set; }
        public required string Question { get; set; }
        [JsonPropertyName("task_line")]
        public TaskLineResponseModel? TaskLine { get; set; }
        public IEnumerable<TaskThemeResponseModel>? Themes { get; set; }
        public TaskSolutionResponseModel? Solution {  get; set; }
    }
}
