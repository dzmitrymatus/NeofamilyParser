namespace NeofamilyParser.DAL.Models
{
    public class TaskEntity
    {
        public int Id { get; set; }
        public required string Section { get; set; }
        public required string Theme { get; set; }
        public int Part { get; set; }
        public int Number { get; set; }
        public required string Source { get; set; }
        public required string QuestionText { get; set; }
        public required string QuestionImages { get; set; }
        public string? Answer { get; set; }
        public required string SolutionText { get; set; }
        public required string SolutionImages { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
