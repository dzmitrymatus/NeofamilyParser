namespace NeofamilyParser.WebAPI.Models
{
    public class TaskApiModel
    {
        public int Id { get; set; }
        public required string Section { get; set; }
        public required string Theme { get; set; }
        public int Part { get; set; }
        public int Number { get; set; }
        public required string Source { get; set; }
        public required string QuestionText { get; set; }
        public required string QuestionImages { get; set; }
        public TaskSolutionApiModel? Solution { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
