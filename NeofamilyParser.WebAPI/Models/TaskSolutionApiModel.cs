namespace NeofamilyParser.WebAPI.Models
{
    public class TaskSolutionApiModel
    {
        public int TaskId { get; set; }
        public string? Answer { get; set; }
        public required string SolutionText { get; set; }
        public required string SolutionImages { get; set; }
    }
}
